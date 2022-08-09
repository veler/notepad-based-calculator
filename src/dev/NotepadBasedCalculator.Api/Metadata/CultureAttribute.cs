namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Defines the culture supported by a component.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Set <see cref="CultureCode"/> to <see cref="Any"/> if the component is culture invariant.
    /// </para>
    /// </remarks>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CultureAttribute : Attribute
    {
        public const string Any = "";
        public const string Arabic = "ar-*";
        public const string English = "en-us";
        public const string EnglishOthers = "en-*";
        public const string Chinese = "zh-cn";
        public const string Spanish = "es-es";
        public const string SpanishMexican = "es-mx";
        public const string Portuguese = "pt-br";
        public const string French = "fr-fr";
        public const string German = "de-de";
        public const string Italian = "it-it";
        public const string Japanese = "ja-jp";
        public const string Dutch = "nl-nl";
        public const string Korean = "ko-kr";
        public const string Swedish = "sv-se";
        public const string Bulgarian = "bg-bg";
        public const string Turkish = "tr-tr";
        public const string Hindi = "hi-in";

        public string CultureCode { get; }

        public CultureAttribute(string cultureCode)
        {
            CultureCode = cultureCode;
        }
    }
}
