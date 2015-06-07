using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Web;

namespace ElFinder
{
    public class Connector : IHttpHandler
    {
        public virtual bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Gets roots array.
        /// </summary>
        public ReadOnlyCollection<IRoot> Roots
        {
            get { return m_roots.AsReadOnly(); }
        }

        /// <summary>
        /// Adds an object to the end of the roots.
        /// </summary>
        /// <param name="item">The root</param>
        public void AddRoot(IRoot item)
        {
            Contract.Requires(item != null);
            m_roots.Add(item);
            item.VolumeId = m_volumePrefix + m_roots.Count + "_";
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            ResponseBase response = GetResponse(context.Request);
            response.WriteResponse(context);
        }
        public Connector(IEnumerable<IRoot> roots)
        {
            m_roots = new List<IRoot>();
            if (roots != null)
                m_roots.AddRange(roots);
        }
        public Connector()
            : this(null)
        {

        }

        internal ResponseBase GetResponse(HttpRequest request)
        {
            Contract.Requires(request != null);
            Contract.Ensures(Contract.Result<ResponseBase>() != null);

            if (m_roots.Count == 0)
                return Errors.ErrorBackend();
            NameValueCollection parameters = request.QueryString.Count > 0 ? request.QueryString : request.Form;
            string commandName = parameters["cmd"];
            if (string.IsNullOrEmpty(commandName))
                return Errors.CommandNotFound();

            string target = parameters["target"];
            if (target != null && target.ToLower() == "null")
                target = null;
            switch (commandName)
            {
                case Commands.Open:
                    if (GetBoolParam(parameters, "init"))
                        return Init(target);
                    else
                        return Open(target, GetBoolParam(parameters, "tree"));
                case Commands.File:
                    return File(target, GetBoolParam(parameters, "download"));
                case Commands.Tree:
                    return Tree(target);
                case Commands.Parents:
                    return Parents(target);
                case Commands.MakeDir:
                    return MakeDir(target, parameters["name"]);
                case Commands.MakeFile:
                    return MakeFile(target, parameters["name"]);
                case Commands.Rename:
                    return Rename(target, parameters["name"]);
                case Commands.Remove:
                    return Remove(GetTargetsArray(request));
                case Commands.List:
                    return List(target);
                case Commands.Get:
                    return Get(target);
                case Commands.Put:
                    return Put(target, parameters["content"]);
                case Commands.Paste:
                    return Paste(parameters["src"], parameters["dst"], GetTargetsArray(request), GetBoolParam(parameters, "cut"));
                case Commands.Upload:
                    return Upload(target, request.Files);
                case Commands.Duplicate:
                    return Duplicate(GetTargetsArray(request));
                case Commands.Thumbnails:
                    return Thumbnails(GetTargetsArray(request));
                case Commands.Dimension:
                    return Dimension(target);
                case Commands.Resize:
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(commandName);
                    string mode = parameters["mode"];
                    if (mode == Commands.Resize)
                        return Resize(target, int.Parse(parameters["width"]), int.Parse(parameters["height"]));
                    if (mode == Commands.Crop)
                        return Crop(target, int.Parse(parameters["x"]), int.Parse(parameters["y"]), int.Parse(parameters["width"]), int.Parse(parameters["height"]));
                    if (mode == Commands.Rotate)
                        return Rotate(target, int.Parse(parameters["degree"]));
                    return Errors.CommandNotFound();
                default:
                    return Errors.CommandNotFound();
            }
        }

        private JsonResponse Open(string target, bool tree)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.Open);
            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();

            OpenResponse response = new OpenResponse(directory);
            response.Files.AddRange(directory.GetUnits().Where(i => !i.IsHidden).Select(i => i.ToDTO()));

            return response;
        }

        private JsonResponse Init(string target)
        {
            IDirectoryInfo directory;
            if (string.IsNullOrEmpty(target))
            {
                IRoot root = m_roots.FirstOrDefault(r => r.StartPath != null);
                if (root == null)
                {
                    if(m_roots.Count == 0)
                        return Errors.NotFound();
                    root = m_roots.First();
                }
                directory = root.StartPath ?? root.Directory;
            }
            else
            {
                directory = ParsePath(target) as IDirectoryInfo;
            }
            if (directory == null)
                return Errors.NotFound();

            InitResponse response = new InitResponse(directory);

            response.Files.AddRange(directory.GetUnits().Where(i => !i.IsHidden).Select(i => i.ToDTO()));
            response.Files.AddRange(m_roots.Select(i => i.Directory.ToDTO()));

            IRoot currentRoot = directory.Root;

            if(directory.RelativePath != string.Empty)
                response.Files.AddRange(currentRoot.Directory.GetDirectories().Where(i => !i.IsHidden).Select(i => i.ToDTO()));

            if (currentRoot.AccessManager.MaxUploadSize.HasValue)
                response.UploadMaxSize = currentRoot.AccessManager.MaxUploadSizeInKb.Value + "K";

            return response;
        }

        private JsonResponse Parents(string target)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.Parents);

            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();
            TreeResponse answer = new TreeResponse();
            if (directory.RelativePath == string.Empty)
            {
                answer.Tree.AddRange(directory.GetDirectories().Where(i => !i.IsHidden).Select(i => i.ToDTO()));  
            }
            else
            {
                IDirectoryInfo parent = directory.Parent;
                while (parent.RelativePath != string.Empty)
                {
                    answer.Tree.AddRange(parent.GetDirectories().Where(i => !i.IsHidden).Select(i => i.ToDTO()));
                    parent = parent.Parent;
                }
                answer.Tree.AddRange(parent.GetDirectories().Where(i => !i.IsHidden).Select(i => i.ToDTO()));                
            }
            answer.Tree.AddRange(m_roots.Select(i => i.Directory.ToDTO()));
            return answer;
        }

        private JsonResponse Tree(string target)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.Tree);

            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();
            TreeResponse answer = new TreeResponse();
            foreach (IDirectoryInfo item in directory.GetDirectories())
            {
                if (!item.IsHidden)
                    answer.Tree.Add(item.ToDTO());
            }
            return answer;
        }

        private JsonResponse List(string target)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.List);

            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();

            ListResponse answer = new ListResponse();
            foreach (IUnitInfo item in directory.GetUnits())
            {
                answer.List.Add(item.Name);
            }
            return answer;
        }

        private JsonResponse MakeDir(string target, string name)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.MakeDir);
            if (string.IsNullOrEmpty(name))
                return Errors.MissedParameter("name");

            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();
            IDirectoryInfo newDir = directory.Root.CreateDirectory(directory.RelativePath, name);
            return new AddResponse(newDir);
        }

        private JsonResponse MakeFile(string target, string name)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.MakeFile);
            if (string.IsNullOrEmpty(name))
                return Errors.MissedParameter("name");

            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();
            IFileInfo newFile = directory.Root.CreateFile(directory.RelativePath, name);
            return new AddResponse(newFile);
        }

        private JsonResponse Rename(string target, string name)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.Rename);
            if (string.IsNullOrEmpty(name))
                return Errors.MissedParameter("name");

            IUnitInfo unit = ParsePath(target);
            ReplaceResponse response = new ReplaceResponse();
            response.Removed.Add(target);

            IUnitInfo renamed;
            if (unit is IDirectoryInfo)
                renamed = unit.Root.RenameDirectory(unit.RelativePath, name);
            else
                renamed = unit.Root.RenameFile(unit.RelativePath, name);

            response.Added.Add(renamed.ToDTO());
            return response;
        }

        private JsonResponse Remove(IEnumerable<string> targets)
        {
            if (targets == null)
                return Errors.MissedParameter("targets");

            RemoveResponse response = new RemoveResponse();
            foreach (string target in targets)
            {
                IUnitInfo item = ParsePath(target);
                response.Removed.Add(target);
                if (item is IDirectoryInfo)
                    item.Root.DeleteDirectory(item.RelativePath);
                else
                    item.Root.DeleteFile(item.RelativePath);
            }
            return response;
        }

        private JsonResponse Duplicate(IEnumerable<string> targets)
        {
            if (targets == null)
                Errors.MissedParameter("targets");

            throw new NotImplementedException();
        }

        private JsonResponse Get(string target)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.Get);
            IFileInfo file = ParsePath(target) as IFileInfo;
            if (file == null || !file.Exists)
                return Errors.NotFound();
            return new GetResponse(file.Root.GetText(file.RelativePath));
        }

        private JsonResponse Put(string target, string content)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.Put);
            if (string.IsNullOrEmpty(content))
                return Errors.MissedParameter("content");

            IFileInfo file = ParsePath(target) as IFileInfo;
            if (file == null || !file.Exists)
                return Errors.NotFound();
            file.Root.PutText(file.RelativePath, content);

            ChangedResponse response = new ChangedResponse();
            response.Changed.Add((FileDTO)file.ToDTO());
            return response;
        }
        private JsonResponse Paste(string source, string dest, IEnumerable<string> targets, bool isCut)
        {
            if (targets == null)
                Errors.MissedParameter("targets");
            if (string.IsNullOrEmpty(source))
                return Errors.MissedParameter("src");
            if (string.IsNullOrEmpty(dest))
                return Errors.MissedParameter("dst");

            throw new NotImplementedException();
        }
        private JsonResponse Upload(string target, HttpFileCollection targets)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.Upload);

            throw new NotImplementedException();
        }

        private JsonResponse Thumbnails(IEnumerable<string> targets)
        {
            if (targets == null)
                Errors.MissedParameter("targets");

            throw new NotImplementedException();
        }

        private JsonResponse Dimension(string target)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.Dimension);

            throw new NotImplementedException();
        }

        private JsonResponse Resize(string target, int width, int height)
        {
            throw new NotImplementedException();
        }

        private JsonResponse Crop(string target, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        private JsonResponse Rotate(string target, int degree)
        {
            throw new NotImplementedException();
        }

        private ResponseBase File(string target, bool download)
        {
            if (string.IsNullOrEmpty(target))
                return Errors.MissedParameter(Commands.File);
            IUnitInfo unit = ParsePath(target);
            if (unit is IDirectoryInfo)
                return new HttpStatusCodeResponse(HttpStatusCode.Forbidden, "You can not download whole folder");
            else
            {
                IFileInfo file = (IFileInfo)unit;
                if (!file.Exists)
                    return new HttpStatusCodeResponse(HttpStatusCode.NotFound, "File not found");
                if (file.Root.AccessManager.IsShowOnly)
                    return new HttpStatusCodeResponse(HttpStatusCode.Forbidden, "Access denied. Volume is for show only");

                return new DownloadResponse(file, download);
            }
        }

        private IUnitInfo ParsePath(string target)
        {
            Contract.Requires(target != null);
            int separatorIndex = target.IndexOf('_');
            if (separatorIndex == -1)
                throw new ArgumentException("Target path must containt a separator between volumeIndex");
            separatorIndex++;
            string volumePrefix = target.Substring(0, separatorIndex);
            string pathHash = target.Substring(separatorIndex);

            IRoot root = m_roots.First(r => r.VolumeId == volumePrefix);
            string relativePath = PathHelper.DecodePath(pathHash);
            IDirectoryInfo dir = root.GetDirectory(relativePath);
            if (dir.Exists)
                return dir;
            else
                return root.GetFile(relativePath);
        }

        private static IEnumerable<string> GetTargetsArray(HttpRequest request)
        {
            string[] targets = request.Form.GetValues("targets");
            NameValueCollection parameters = request.QueryString.Count > 0 ? request.QueryString : request.Form;
            if (targets == null)
            {
                string t = parameters["targets[]"];
                if (string.IsNullOrEmpty(t))
                    t = parameters["targets"];
                if (string.IsNullOrEmpty(t))
                    return null;
                targets = t.Split(',');
            }
            return targets;
        }

        private static bool GetBoolParam(NameValueCollection parameters, string name)
        {
            return !string.IsNullOrEmpty(parameters[name]) && parameters[name] == "1";
        }
        
        private readonly List<IRoot> m_roots;
        private readonly string m_volumePrefix = "v";
    }
}
