using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using CsvHelper;
using CsvHelper.Configuration;

class Program
{
    static void Main(string[] args)
    {
        string csvFilePath = @"C:\dev\templates.csv";
        var templates = ReadTemplatesFromCsv(csvFilePath);
        
        // Exemple de dictionnaire d'entrée
        var templatesFromDict = GetDictionnary();

        templates.AddRange(templatesFromDict);

        // Chemin du fichier existant
        string cSharpTemplateFile = @"C:\Users\FabienBIGUET\AppData\Roaming\JetBrains\Rider2023.2\resharper-host\GlobalSettingsStorage.DotSettings";
        string sqlTemplateFile = @"C:\Users\FabienBIGUET\AppData\Roaming\JetBrains\Rider2023.2\templates\SQL.xml";

        // Ajouter les nouveaux templates au fichier existant
        AddCsharpTemplates(templates, cSharpTemplateFile);
        AddSqlTemplates( templates, sqlTemplateFile);
    }

    private static Dictionary<string, string> GetDictionnary()
    {
        var templatesFromDict = new Dictionary<string, string>
        {
            { "superCrevette", "071BB20F-9186-4D27-A10B-E465E6C55DF1" },
            { "fbiguet", "d3f09f2f-8974-44f1-89ea-bf71abefde3e" },
            { "Apec", "7A03F986-8DA7-4724-A830-898E0BB9D31A" },
            { "ArjowigginsSarthe", "e907c802-e223-4180-845d-8d22664194cd" },
            { "AuvergneRhoneAlpes", "DD393DB7-44D4-45A2-87B7-C757288983CB" },
            { "CareerPage", "70202742-AAC7-41EA-8080-AB5568B6FFCC" },
            { "Cooptation", "6D038C1B-F64B-4E30-9A31-59789CD1B3E1" },
            { "EmploiCantal", "48F8E56E-BB43-422F-BAC9-5CE13FDDD703" },
            { "EspaceEmploiSaintDizier", "1ED9AC92-A050-4867-9320-06ECF5F14FE3" },
            { "FederationHotelleriePleinAir", "E4F1E2DE-7FB5-4EDA-8DEF-741625D37D45" },
            { "FigaroEmploi", "B6298320-0791-42E1-96AF-D65F9B78BBA3" },
            // {"FrenchTex" , "835A8F64-A84C-4E83-BEAE-6DFD3BFAEA08"},
            { "Glassdoor", "2994A02A-D56D-48E5-8529-8F334479D1A0" },
            { "GoogleForJobs", "b4013b5f-e23f-4b98-96f3-114e9ec1ddb2" },
            { "Indeed", "F51ACB7F-C488-44C7-917C-089A899101AB" },
            { "ImpulseEmploi", "41CC4CFD-249D-4D0A-BBD2-82D9AD0FB446" },
            { "JeChercheUnEmploi", "8C8910B8-26A3-4D3D-AC96-8313C2886C11" },
            { "JobDoe", "38391145-2B7C-4A03-BC4A-870C360499A7" },
            { "JobExchange", "28E0D848-7207-49AC-A63D-DE3F9D569EF9" },
            { "JobIsJob", "714154FE-8083-4C63-B30C-7415471A0BC7" },
            { "Jobrapido", "F527E1DA-4434-4B72-B541-A4970C02ACB1" },
            { "Jobtome", "CF8A2E32-D565-4385-BD41-59D8A3133C91" },
            { "Jooble", "321AB48C-2864-4852-A7FB-AE5330646B5C" },
            { "Linkedin", "66D7E8BD-800B-4B50-9CA1-1B34984FD9A3" },
            { "NosEmplois", "FCF22AC0-61D7-42B8-B144-928E1605700E" },
            { "MobiliteInterne", "5CBA1C04-03EA-476A-AA73-DEAA7CE0E8C0" },
            { "MonEmploi77", "B57604BF-B89D-4E8E-8D5F-CDA2624CADF0" },
            { "MonsterFree", "698B9806-7D60-4D56-AB85-FD4DA8573845" },
            { "OptionCarriere", "BB461BB0-4816-4331-B485-011938964CBA" },
            { "PaysDeCraon", "6afb5e53-26f4-4975-9b90-a510b66186a0" },
            { "PaysDeLaLoire", "E7E529C1-781C-478D-A07B-6E07F4AE94C0" },
            { "PoleEmploi", "ec47dc09-22fa-49df-8990-c71f26b00168" },
            { "PositivEmploi", "3B3EC12C-64EB-4655-9A41-9A3A0D408A8B" },
            { "SesameEmploi", "CCF97E24-AEDC-4D80-8B8E-6C380A380C90" },
            { "SimplyHired", "CDFD3BD5-0C05-437E-87FF-597A2E494B03" },
            { "Talent", "6CE968DA-DA94-437D-8063-EECC6224F1F2" },
            { "ToulouseMetropole", "46A45DEF-97B7-4B4E-9F5F-ECF80350B5B3" },
            { "Jobijoba", "8556B5D2-2213-41A9-8FF1-9226D0FF4653" },
            { "SmartForum", "D14135EC-45EC-477A-A29E-EA7DA7A2AD2E" },
            { "Agefiph", "90854456-DF61-4030-94D4-496091B27E83" },
            { "ThousandAndOneInterims", "5F073D89-8BF0-4255-9F88-E2ED122EF380" },
            { "AeroEmploiFormation", "C96D5000-982A-4F74-BACB-02BAE31500FB" },
            { "AeroContact", "7A71F4B6-0201-4BE3-9872-CEE5B53352FC" },
            { "Aladom", "55909EE5-C00B-4626-9218-D8570229125A" },
            { "Apecita", "73FC4447-47BB-4B07-BA57-772A62542B30" },
            { "ApecitaStage", "C5F51D5E-ABF0-45E8-9AF0-6CF0A1739D59" },
            { "Basile", "AC4D1F9B-7254-46CB-B0C6-75346F320575" },
            { "Cadremploi", "912D50C1-32E2-43ED-AD89-0E89DB2BF242" },
            { "Cookorico", "89822472-49D9-46A9-8C51-F4414A7FAF1C" },
            { "Crechemploi", "AD1C6676-BE64-4F65-A73C-9B06C9292177" },
            { "CVCatcher", "AAA6869F-9865-41AA-841B-9F42277F0767" },
            { "DomTomJob", "E67BB7C4-C037-425E-8C5B-981EA9D6664C" },
            { "EmploiCollectivites", "E824D50F-EC87-4E62-AE84-BC5D95E0BB28" },
            { "FashionJobs", "088CA947-2656-485B-A8EB-7BC7301B39D9" },
            { "FashionJobsStage", "8D0FF66A-1A7A-4D69-8E0E-D1362AE10FA5" },
            { "FrenchTex", "44A7E5C6-5181-446E-B554-62522FD691E3" },
            { "GestionCamping", "3CE55E1A-7D5E-41F5-B691-58E8D06D9AAB" },
            { "GoldenBees", "3AC02EAD-BBC9-4CCD-8F21-13E082133F3C" },
            { "Holeest", "518668B7-B244-4A06-8B75-4CA39B193F7E" },
            { "Hellowork", "AB4EE25A-E4F8-41F5-BEBD-BD364006ECB8" },
            { "HotellerieRestauration", "F7BAF3C1-86B7-4627-AF89-6646F4653157" },
            { "HotellerieRestaurationStage", "4FE9C905-015B-4356-8D63-0C2096946C05" },
            { "IndeedSponsoring", "270C4079-9245-4CDC-B5FD-8FF9DA0E4530" },
            { "Jobeo", "F72B1567-4E6E-4953-9AE7-EF29CD030204" },
            { "JobMarketingVente", "51A915BF-22B7-43C4-80CD-6A06B8A4F41D" },
            { "JobTeaser", "226E7D63-93BE-4F47-8850-3172C13F9C9D" },
            { "JobTransport", "AE2F33B7-8D9C-407C-BD29-ECD515270EDF" },
            { "JournalDesPalaces", "7D392C45-C7C8-4259-9C50-A2909FBB0D57" },
            { "Lamacompta", "B8BDAD2E-BA1A-4A54-928A-12C3ED7A96EE" },
            { "Leboncoin", "5E61055E-C011-44D3-89A0-F43E5E671A13" },
            { "LesJeudis", "DB3731D8-F97C-4813-9282-3A1E7B0BB1FA" },
            // {"MeteoJob" , "B27D066A-14E3-452C-9102-F96983F7078C"},
            { "Monster", "96F024D2-B41B-4CEA-A35D-7CF939AAC33F" },
            { "ObservatoireDeLaFranchise", "E1B678D7-859B-4701-8E8C-2BEE89257DD1" },
            { "OuestFranceEmploi", "1CDA0DD7-83DB-4904-A043-DDBB4E9506BA" },
            { "StaffSante", "05E7F75C-4232-4210-879B-D899FCF99DF4" },
            { "Turijobs", "B2CAB2B5-A66F-4853-AC12-18DC254B5CFB" },
            { "Ush", "007277ce-4426-45c9-b286-dd885c51aae4" },
            { "Vitijob", "EC48F3E9-75F7-4EAA-BA1E-205E4E154884" },
            { "VoRH", "0F3CBBA7-96A2-4C6C-8874-C6E442D425CA" },
            { "WelcomeToTheJungle", "29DBABFE-B714-4DC7-862B-FCEA62AE3A4D" },
        };
        return templatesFromDict;
    }

    static Dictionary<string, string> ReadTemplatesFromCsv(string csvFilePath)
    {
        var templates = new Dictionary<string, string>();

        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
               {
                   Delimiter = ";", // Définir le séparateur comme point-virgule
                   HeaderValidated = null,
                   MissingFieldFound = null
               }))
        {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var name = csv.GetField<string>("name");
                var template = csv.GetField<string>("template");
                templates.Add(name, template);
            }
        }

        return templates;
    }

    static void AddSqlTemplates(Dictionary<string, string> templates, string filePath)
    {
        // Vérifier si le fichier existe
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Le fichier {filePath} n'existe pas.");
            return;
        }

        // Charger le document XML existant
        XDocument document = XDocument.Load(filePath);

        // Récupérer l'élément templateSet
        XElement templateSet = document.Root;

        if (templateSet == null || templateSet.Name != "templateSet")
        {
            Console.WriteLine($"Le fichier {filePath} ne contient pas un élément templateSet valide.");
            return;
        }

        // Ajouter les nouveaux templates
        foreach (var template in templates)
        {
            XElement newTemplate = new XElement("template",
                new XAttribute("name", template.Key.ToLower()),
                new XAttribute("value", $"'{template.Value}'"),
                new XAttribute("description", $"{template.Key.ToLower()} template"),
                new XAttribute("toReformat", "true"),
                new XAttribute("toShortenFQNames", "false"));

            newTemplate.Add(new XElement("variable",
                new XAttribute("name", "col"),
                new XAttribute("expression", ""),
                new XAttribute("defaultValue", "&quot;col&quot;"),
                new XAttribute("alwaysStopAt", "true")));

            newTemplate.Add(new XElement("variable",
                new XAttribute("name", "type"),
                new XAttribute("expression", ""),
                new XAttribute("defaultValue", "&quot;int&quot;"),
                new XAttribute("alwaysStopAt", "true")));

            newTemplate.Add(new XElement("variable",
                new XAttribute("name", "null"),
                new XAttribute("expression", ""),
                new XAttribute("defaultValue", "&quot;not null&quot;"),
                new XAttribute("alwaysStopAt", "true")));

            XElement context = new XElement("context");
            context.Add(new XElement("option",
                new XAttribute("name", "SQL_CODE"),
                new XAttribute("value", "true")));
            
            newTemplate.Add(context);

            templateSet.Add(newTemplate);
        }

        // Sauvegarder le fichier XML mis à jour
        document.Save(filePath);
        Console.WriteLine($"Le fichier {filePath} a été mis à jour avec succès.");
    }


    static void AddCsharpTemplates(Dictionary<string, string> templates, string filePath)
    {
        // Vérifier si le fichier existe
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Le fichier {filePath} n'existe pas.");
            return;
        }

        // Charger le document XML existant
        XDocument document = XDocument.Load(filePath);

        // Déterminer le namespace utilisé dans le document
        XNamespace wpfNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        XNamespace xNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
        XNamespace sNamespace = "clr-namespace:System;assembly=mscorlib";

        // Récupérer l'élément ResourceDictionary
        XElement resourceDictionary = document.Root;

        if (resourceDictionary == null)
        {
            resourceDictionary = new XElement(wpfNamespace + "ResourceDictionary");
            document.Add(resourceDictionary);
        }

        // Ajouter les nouveaux templates
        foreach (var template in templates)
        {
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            string scopeGuid = Guid.NewGuid().ToString("N").ToUpper();

            resourceDictionary.Add(new XElement(sNamespace + "Boolean",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/@KeyIndexDefined"),
                true));

            resourceDictionary.Add(new XElement(sNamespace + "Boolean",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/Applicability/=Live/@EntryIndexedValue"),
                true));

            resourceDictionary.Add(new XElement(sNamespace + "String",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/Description/@EntryValue"),
                $"{template.Key.ToLower()} template"));

            resourceDictionary.Add(new XElement(sNamespace + "Boolean",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/Reformat/@EntryValue"),
                true));

            resourceDictionary.Add(new XElement(sNamespace + "Boolean",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/Scope/={scopeGuid}/@KeyIndexDefined"),
                true));

            resourceDictionary.Add(new XElement(sNamespace + "String",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/Scope/={scopeGuid}/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue"),
                "2.0"));

            resourceDictionary.Add(new XElement(sNamespace + "String",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/Scope/={scopeGuid}/Type/@EntryValue"),
                "InCSharpFile"));

            resourceDictionary.Add(new XElement(sNamespace + "String",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/Shortcut/@EntryValue"),
                template.Key.ToLower()));

            resourceDictionary.Add(new XElement(sNamespace + "Boolean",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/ShortenQualifiedReferences/@EntryValue"),
                true));

            resourceDictionary.Add(new XElement(sNamespace + "String",
                new XAttribute(xNamespace + "Key", $"/Default/PatternsAndTemplates/LiveTemplates/Template/={guid}/Text/@EntryValue"),
                template.Value));
        }

        // Sauvegarder le fichier XML mis à jour
        document.Save(filePath);
        Console.WriteLine($"Le fichier {filePath} a été mis à jour avec succès.");
    }
}

public static class DictionaryExtensions
{
    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        foreach (var kvp in source)
        {
            target[kvp.Key] = kvp.Value;
        }
    }
}
