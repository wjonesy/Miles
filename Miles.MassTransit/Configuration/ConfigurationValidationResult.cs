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

        public ValidationResultDisposition Disposition { get; private set; }

        public string Key { get; private set; }

        public string Message { get; private set; }

        public string Value { get; private set; }
    }
}