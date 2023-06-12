namespace Domain.Models.Utils
{
    public static class PANUtils
    {
        public static string MaskPan(string pan) => "****-****-****-" + pan.Split("-")[3];
    }
}
