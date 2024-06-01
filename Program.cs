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
        string csvFilePath = @"D:\dev\templates.csv";
        var templates = ReadTemplatesFromCsv(csvFilePath);
        
        // Exemple de dictionnaire d'entrée
        var templatesFromDict = new Dictionary<string, string>
        {
            // { "meteojob3", "AAGGG-JKJHD-KJHDJKH-456" },
            // { "log", "console.log('$END$');" }
            { "titi", "console.log('titi');" }
        };

        templates.AddRange(templatesFromDict);

        // Chemin du fichier existant
        string filePath = @"C:\Users\Fabien\AppData\Roaming\JetBrains\Rider2023.3\resharper-host\GlobalSettingsStorage.DotSettings";

        // Ajouter les nouveaux templates au fichier existant
        AddTemplatesToExistingFile(templates, filePath);
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



    static void AddTemplatesToExistingFile(Dictionary<string, string> templates, string filePath)
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
                $"{template.Key} template"));

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
                template.Key));

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
