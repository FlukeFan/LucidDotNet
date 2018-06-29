using System;

namespace Lucid.ProjectCreation
{
    public class ExecutableValidator
    {
        public static void Validate(object command)
        {

        }
    }

    public class FacadeException : Exception
    {
    }

    public class GenerateProject
    {
        public string Name { get; set; }

        public byte[] Execute()
        {
            return null;
        }
    }
}
