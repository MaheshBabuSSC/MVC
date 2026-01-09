using Microsoft.EntityFrameworkCore;


namespace MvcWebApiSwaggerApp.Models


{
    public class FormDefinition
    {
        public int Id { get; set; }
        public string FormName { get; set; } = string.Empty;
        public List<FormField> Fields { get; set; } = new();
    }

    public class FormField
    {
        public string FieldName { get; set; } = string.Empty;
        public string FieldType { get; set; } = string.Empty;
         public string Options { get; set; } // For select fields
    public int FieldOrder { get; set; }
        // text, number, email, date
    }

    public class FormResponse
    {
        public int FormId { get; set; }
        public Dictionary<string, string> Values { get; set; } = new();
    }

    public class CreateFormRequest
    {
        public string FormTitle { get; set; }
        public string FormDescription { get; set; }
        public string CreatedBy { get; set; }
        public List<FormField> Fields { get; set; }
    }
    [Keyless]
    public class FormSummary
    {
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public string FormDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

    }


}
