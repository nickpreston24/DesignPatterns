using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Common
{
    public class ApiMethodController : ApiController
    {

        /// <summary>
        /// TODO: Put this method in an API.  Treat as a stub until it can be used.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApiMethod> GetMethods()
        {
            // get the IApiExplorer registered automatically
            var explorer = Configuration.Services.GetApiExplorer();

            // loop, convert and return all descriptions 
            return explorer.ApiDescriptions
                // ignore self
                .Where(descriptor => descriptor.ActionDescriptor.ControllerDescriptor.ControllerName != "ApiMethod")
                .Select(desc =>
                {
                    // convert to a serializable structure
                    return new ApiMethod
                    {
                        Parameters = desc.ParameterDescriptions.Select(p => new ApiMethodParameter
                        {
                            Name = p.Name,
                            Type = p.ParameterDescriptor.ParameterType.FullName,
                            IsOptional = p.ParameterDescriptor.IsOptional
                        }).ToArray(),
                        Method = desc.HttpMethod.ToString(),
                        RelativePath = desc.RelativePath,
                        //ReturnType = desc.ResponseDescription.DeclaredType == null ?
                        //    null : desc.ResponseDescription.DeclaredType.ToString()
                    };
                });
        }
    }

    public class ApiMethod
    {
        public string Method { get; set; }
        public string RelativePath { get; set; }
        public string ReturnType { get; set; }
        public IEnumerable<ApiMethodParameter> Parameters { get; set; }
    }

    public class ApiMethodParameter
    {
        public bool IsOptional { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

    }
}
