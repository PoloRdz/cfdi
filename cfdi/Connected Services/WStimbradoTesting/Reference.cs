﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WStimbradoTesting
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.1-preview-30310-0943")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:respuestaTimbrado", ConfigurationName="WStimbradoTesting.generaCFDIPortType")]
    public interface generaCFDIPortType
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost/servicetomza/generaCFDI.php/generaCFDI/generaCFDI/generaCFDI", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<WStimbradoTesting.respuestaTimbrado> generaCFDIAsync(string usuario, string password, string documentoXML);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.1-preview-30310-0943")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:respuestaTimbrado")]
    public partial class respuestaTimbrado
    {
        
        private string documentoProcesadoField;
        
        private string codigoResultadoField;
        
        private string codigoDescripcionField;
        
        /// <remarks/>
        public string documentoProcesado
        {
            get
            {
                return this.documentoProcesadoField;
            }
            set
            {
                this.documentoProcesadoField = value;
            }
        }
        
        /// <remarks/>
        public string codigoResultado
        {
            get
            {
                return this.codigoResultadoField;
            }
            set
            {
                this.codigoResultadoField = value;
            }
        }
        
        /// <remarks/>
        public string codigoDescripcion
        {
            get
            {
                return this.codigoDescripcionField;
            }
            set
            {
                this.codigoDescripcionField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.1-preview-30310-0943")]
    public interface generaCFDIPortTypeChannel : WStimbradoTesting.generaCFDIPortType, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.1-preview-30310-0943")]
    public partial class generaCFDIPortTypeClient : System.ServiceModel.ClientBase<WStimbradoTesting.generaCFDIPortType>, WStimbradoTesting.generaCFDIPortType
    {
        
        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public generaCFDIPortTypeClient() : 
                base(generaCFDIPortTypeClient.GetDefaultBinding(), generaCFDIPortTypeClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.generaCFDIPort.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public generaCFDIPortTypeClient(EndpointConfiguration endpointConfiguration) : 
                base(generaCFDIPortTypeClient.GetBindingForEndpoint(endpointConfiguration), generaCFDIPortTypeClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public generaCFDIPortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(generaCFDIPortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public generaCFDIPortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(generaCFDIPortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public generaCFDIPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<WStimbradoTesting.respuestaTimbrado> generaCFDIAsync(string usuario, string password, string documentoXML)
        {
            return base.Channel.generaCFDIAsync(usuario, password, documentoXML);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.generaCFDIPort))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.generaCFDIPort))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost/servicetomza/generaCFDI.php/generaCFDI/generaCFDI");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return generaCFDIPortTypeClient.GetBindingForEndpoint(EndpointConfiguration.generaCFDIPort);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return generaCFDIPortTypeClient.GetEndpointAddress(EndpointConfiguration.generaCFDIPort);
        }
        
        public enum EndpointConfiguration
        {
            
            generaCFDIPort,
        }
    }
}
