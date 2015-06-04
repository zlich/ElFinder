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
            string cmdName = parameters["cmd"];
            if (string.IsNullOrEmpty(cmdName))
                return Errors.CommandNotFound();

            string target = parameters["target"];
            if (target != null && target.ToLower() == "null")
                target = null;
            switch (cmdName)
            {
                case "open":
                    if (!string.IsNullOrEmpty(parameters["init"]) && parameters["init"] == "1")
                    {
                        return Init(target);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        return Open(target, !string.IsNullOrEmpty(parameters["tree"]) && parameters["tree"] == "1");
                    }
                case "file":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return File(target, !string.IsNullOrEmpty(parameters["download"]) && parameters["download"] == "1");
                case "tree":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return Tree(target);
                case "parents":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return Parents(target);
                case "mkdir":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        string name = parameters["name"];

                        if (string.IsNullOrEmpty(name))
                            return Errors.MissedParameter("name");
                        return MakeDir(target, name);
                    }
                case "mkfile":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        string name = parameters["name"];

                        if (string.IsNullOrEmpty(name))
                            return Errors.MissedParameter("name");
                        return MakeFile(target, name);
                    }
                case "rename":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        string name = parameters["name"];

                        if (string.IsNullOrEmpty(name))
                            return Errors.MissedParameter("name");
                        return Rename(target, name);
                    }
                case "rm":
                    {
                        IEnumerable<string> targets = GetTargetsArray(request);
                        if (targets == null)
                            return Errors.MissedParameter("targets");
                        return Remove(targets);
                    }
                case "ls":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return List(target);
                case "get":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return Get(target);
                case "put":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    string content = parameters["content"];

                    if (string.IsNullOrEmpty(content))
                        return Errors.MissedParameter("content");
                    return Put(target, content);
                case "paste":
                    {
                        IEnumerable<string> targets = GetTargetsArray(request);
                        if (targets == null)
                            Errors.MissedParameter("targets");
                        string src = parameters["src"];
                        if (string.IsNullOrEmpty(src))
                            return Errors.MissedParameter("src");

                        string dst = parameters["dst"];
                        if (string.IsNullOrEmpty(dst))
                            return Errors.MissedParameter("dst");

                        return Paste(src, dst, targets, !string.IsNullOrEmpty(parameters["cut"]) && parameters["cut"] == "1");
                    }
                case "upload":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return Upload(target, request.Files);
                case "duplicate":
                    {
                        IEnumerable<string> targets = GetTargetsArray(request);
                        if (targets == null)
                            Errors.MissedParameter("targets");
                        return Duplicate(targets);
                    }
                case "tmb":
                    {
                        IEnumerable<string> targets = GetTargetsArray(request);
                        if (targets == null)
                            Errors.MissedParameter("targets");
                        return Thumbs(targets);
                    }
                case "dim":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        return Dim(target);
                    }
                case "resize":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        switch (parameters["mode"])
                        {
                            case "resize":
                                return Resize(target, int.Parse(parameters["width"]), int.Parse(parameters["height"]));
                            case "crop":
                                return Crop(target, int.Parse(parameters["x"]), int.Parse(parameters["y"]), int.Parse(parameters["width"]), int.Parse(parameters["height"]));
                            case "rotate":
                                return Rotate(target, int.Parse(parameters["degree"]));
                            default:
                                break;
                        }
                        return Errors.CommandNotFound();
                    }
                default:
                    return Errors.CommandNotFound();
            }
        }

        private JsonResponse Open(string target, bool tree)
        {
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
            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();
            IDirectoryInfo newDir = directory.Root.CreateDirectory(directory.RelativePath, name);
            return new AddResponse(newDir);
        }

        private JsonResponse MakeFile(string target, string name)
        {
            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();
            IFileInfo newFile = directory.Root.CreateFile(directory.RelativePath, name);
            return new AddResponse(newFile);
        }

        private JsonResponse Rename(string target, string newName)
        {
            IUnitInfo unit = ParsePath(target);
            ReplaceResponse response = new ReplaceResponse();
            response.Removed.Add(target);

            IUnitInfo renamed;
            if (unit is IDirectoryInfo)
                renamed = unit.Root.RenameDirectory(unit.RelativePath, newName);
            else
                renamed = unit.Root.RenameFile(unit.RelativePath, newName);

            response.Added.Add(renamed.ToDTO());
            return response;
        }

        private JsonResponse Remove(IEnumerable<string> targets)
        {
            Contract.Requires(targets != null);

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
            throw new NotImplementedException();
        }

        private JsonResponse Get(string target)
        {
            throw new NotImplementedException();
        }

        private JsonResponse Put(string target, string content)
        {
            throw new NotImplementedException();
        }
        private JsonResponse Paste(string source, string dest, IEnumerable<string> targets, bool isCut)
        {
            throw new NotImplementedException();
        }
        private JsonResponse Upload(string target, HttpFileCollection targets)
        {
            throw new NotImplementedException();
        }

        private JsonResponse Thumbs(IEnumerable<string> targets)
        {
            throw new NotImplementedException();
        }

        private JsonResponse Dim(string target)
        {
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

        private IEnumerable<string> GetTargetsArray(HttpRequest request)
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
        
        private readonly List<IRoot> m_roots;
        private readonly string m_volumePrefix = "v";
    }
}
