using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Web;

namespace ElFinder
{
    public class Connector
    {
        public Connector(Driver driver)
        {
            Contract.Requires(driver != null);
            _driver = driver;
        }

        public ResponseBase GetResponse(HttpRequest request)
        {
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
                        return _driver.Init(target);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        return _driver.Open(target, !string.IsNullOrEmpty(parameters["tree"]) && parameters["tree"] == "1");
                    }
                case "file":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return _driver.File(target, !string.IsNullOrEmpty(parameters["download"]) && parameters["download"] == "1");
                case "tree":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return _driver.Tree(target);
                case "parents":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return _driver.Parents(target);
                case "mkdir":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        string name = parameters["name"];

                        if (string.IsNullOrEmpty(name))
                            return Errors.MissedParameter("name");
                        return _driver.MakeDir(target, name);
                    }
                case "mkfile":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        string name = parameters["name"];

                        if (string.IsNullOrEmpty(name))
                            return Errors.MissedParameter("name");
                        return _driver.MakeFile(target, name);
                    }
                case "rename":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        string name = parameters["name"];

                        if (string.IsNullOrEmpty(name))
                            return Errors.MissedParameter("name");
                        return _driver.Rename(target, name);
                    }
                case "rm":
                    {
                        IEnumerable<string> targets = GetTargetsArray(request);
                        if (targets == null)
                            Errors.MissedParameter("targets");
                        return _driver.Remove(targets);
                    }
                case "ls":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return _driver.List(target);
                case "get":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return _driver.Get(target);
                case "put":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    string content = parameters["content"];

                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter("content");
                    return _driver.Put(target, content);
                case "paste":
                    {
                        IEnumerable<string> targets = GetTargetsArray(request);
                        if (targets == null)
                            Errors.MissedParameter("targets");
                        string src = parameters["src"];
                        if (string.IsNullOrEmpty(src))
                            return Errors.MissedParameter("src");

                        string dst = parameters["dst"];
                        if (string.IsNullOrEmpty(src))
                            return Errors.MissedParameter("dst");

                        return _driver.Paste(src, dst, targets, !string.IsNullOrEmpty(parameters["cut"]) && parameters["cut"] == "1");
                    }
                case "upload":
                    if (string.IsNullOrEmpty(target))
                        return Errors.MissedParameter(cmdName);
                    return _driver.Upload(target, request.Files);
                case "duplicate":
                    {
                        IEnumerable<string> targets = GetTargetsArray(request);
                        if (targets == null)
                            Errors.MissedParameter("targets");
                        return _driver.Duplicate(targets);
                    }
                case "tmb":
                    {
                        IEnumerable<string> targets = GetTargetsArray(request);
                        if (targets == null)
                            Errors.MissedParameter("targets");
                        return _driver.Thumbs(targets);
                    }
                case "dim":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        return _driver.Dim(target);
                    }
                case "resize":
                    {
                        if (string.IsNullOrEmpty(target))
                            return Errors.MissedParameter(cmdName);
                        switch (parameters["mode"])
                        {
                            case "resize":
                                return _driver.Resize(target, int.Parse(parameters["width"]), int.Parse(parameters["height"]));
                            case "crop":
                                return _driver.Crop(target, int.Parse(parameters["x"]), int.Parse(parameters["y"]), int.Parse(parameters["width"]), int.Parse(parameters["height"]));
                            case "rotate":
                                return _driver.Rotate(target, int.Parse(parameters["degree"]));
                            default:
                                break;
                        }
                        return Errors.CommandNotFound();
                    }
                default:
                    return Errors.CommandNotFound();
            }
        }

        private IEnumerable<string> GetTargetsArray(HttpRequest request)
        {
            IEnumerable<string> targets = request.Form.GetValues("targets");
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

        private readonly Driver _driver;
    }
}
