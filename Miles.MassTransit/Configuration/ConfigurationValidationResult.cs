using MassTransit.Configurators;

namespace Miles.MassTransit.Configuration
{
    class ConfigurationValidationResult : ValidationResult
    {
        public ConfigurationValidationResult(
            ValidationResultDisposition disposition,
            string key,
            string message,
            string value)
        {
            this.Disposition = disposition;
            this.Key = key;
            this.Message = message;
            this.Value = value;
        }

        public ValidationResultDisposition Disposition { get; set; }

        public string Key { get; set; }

        public string Message { get; set; }

        public string Value { get; set; }
    }
}