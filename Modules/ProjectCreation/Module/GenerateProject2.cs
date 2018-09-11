namespace Lucid.Modules.ProjectCreation
{
    //public class GenerateProject : Command<byte[]>
    //{
    //    [Required(ErrorMessage = "Please supply a Name")]
    //    public string Name { get; set; }

    //    public override byte[] Execute()
    //    {
    //        ValidateName();

    //        var assembly = GetType().Assembly;

    //        using (var zipInputStream = assembly.GetManifestResourceStream("Lucid.Web.Project.zip"))
    //        using (var zipOutputStream = Generate.Project(zipInputStream, Name))
    //            return StreamToBytes(zipOutputStream);
    //    }

    //    private static byte[] StreamToBytes(Stream stream)
    //    {
    //        using (var memory = new MemoryStream())
    //        {
    //            stream.CopyTo(memory);
    //            return memory.ToArray();
    //        }
    //    }

    //    private void ValidateName()
    //    {
    //        ValidateStartsWithAlpha();
    //        ValidateNoSpaces();
    //        ValidateNoSpecialCharacters();
    //    }

    //    private void ValidateStartsWithAlpha()
    //    {
    //        if (!char.IsLetter(Name[0]))
    //            throw this.PropertyError(gp => gp.Name, p => "Please supply a Name that starts with a letter");
    //    }

    //    private void ValidateNoSpaces()
    //    {
    //        if (Name.Contains(" "))
    //            throw this.PropertyError(gp => gp.Name, p => "Please supply a Name that does not contains spaces");
    //    }

    //    private void ValidateNoSpecialCharacters()
    //    {
    //        foreach (var chr in Name)
    //            if (!char.IsLetterOrDigit(chr) && chr != '_')
    //                throw this.PropertyError(gp => gp.Name, p => "Please supply a Name that does not contains special characters");
    //    }
    //}
}