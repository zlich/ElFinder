namespace ElFinder
{
    /// <summary>
    /// Represents factory for errors.
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
        public static ErrorResponse NotFound()
        {
            return new ErrorResponse("File not found");
        }
        public static ErrorResponse FolderLocked(string forderName)
        {
            return new ErrorResponse("errLocked", forderName);
        }
        public static ErrorResponse MissedParameter(string command)
        {
            return new ErrorResponse("errCmdParams", command);
        }
        public static ErrorResponse ErrorBackend()
        {
            return new ErrorResponse("Invalid backend configuration. Roots count equals zero.");
        }
    }
}