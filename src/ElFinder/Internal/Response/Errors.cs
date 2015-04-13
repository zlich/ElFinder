﻿namespace ElFinder
{
    /// <summary>
    /// Represents factory for errors
    /// </summary>
    internal static class Errors
    {
        public static ErrorResponse CommandNotFound()
        {
            return new ErrorResponse("errUnknownCmd");
        }
        public static ErrorResponse CannotUploadFile()
        {
            return new ErrorResponse("errUploadFile");
        }
        public static ErrorResponse MaxUploadFileSize()
        {
            return new ErrorResponse("errFileMaxSize");
        }
        public static ErrorResponse AccessDenied()
        {
            return new ErrorResponse("errAccess");
        }
        public static ErrorResponse MissedParameter(string command)
        {
            return new ErrorResponse("errCmdParams", command);
        }
    }
}