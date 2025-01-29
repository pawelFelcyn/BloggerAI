namespace BloggerAI.Core.Services;

public sealed class WritePostChatMessageFactory
{
    public string GetChatMessage(string transcription, string lang, PostFormat postFormat)
    {
        if (lang == "pl-PL")
        {
            return GetPolishChatMessage(transcription, postFormat);
        }

        throw new NotSupportedException($"Not supported lang: {lang}");
    }

    private string GetPolishChatMessage(string transcription, PostFormat postFormat)
    {
        return $"""
            Utworzyłem filmik na pewnej platformie do wrzucania wideo, jak np. YouTube. W tej wiadomości, poniżej prześlę ci jego transkrypcję.
            Twoim zadaniem będzie stworzenie wpisu na bloga poruszającego temat zawarty w filmiku. Niektórzy twórcy prowadzą obok swojego 
            kanału na np. YouTube blog, gdzie dodatkowo umieszczają wpisy/artykuły na poruszane na kanale tematy. Prowadzę właśnie coś takiego,
            a Ty jesteś moim osobistym redaktorem AI. Wpis utwórz w formacie {postFormat}, PAMIĘTAJ O WSZELKICH ZNAKACH SPECJALNYCH ZWIĄZANYCH Z TYM FORMATEM.
            Wpis ten nie ma być jedynie streszczeniem, wprowadzeniem, ani podsumowaniem filmiku. Ma on być sam w sobie pełnoprawnym wpisem/artykułem.
            Pamiętaj o przeredagowaniu transkrypcji w sposób właściwy dla tekstów pisanych. Podziel odpowiednio na rozdziały itp. Pierwsza linijka tekstu niech będzie tytułem.
            Pamiętaj również, aby w tekście pominąć np. reklamy, prośby o subskrybcje, czy inne nieistotne wstawki. 
            TERAZ BARDZO WAŻNA CZĘŚĆ - w swojej odpowiedzi napisz WYŁĄCZNIE wpis, bez własnych wprowadzeń w stylu "Rozumiem, oto wpis", ani bez żadnych własnych zakończeń
            w stylu "Daj znać czy ten wpis jest ok". To szalenie istotne, ponieważ rozmawiam teraz z Tobą poprzez API i nie jestem w stanie nic po Tobie porawić, czy niczego wyciąć.
            Twoja odpowiedź zostanie W CAŁOŚCI przesłana na bloga jako artykuł. Oto tekst trakskrypcji filmu:{Environment.NewLine}{transcription} 
            """;
    }
}