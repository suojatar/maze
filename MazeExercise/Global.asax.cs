using System.Web;
using System.Web.Http;

using Newtonsoft.Json.Serialization;

namespace MazeExercise
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);

			//Configure JSON to generate property names in Camel Case
			HttpConfiguration config = GlobalConfiguration.Configuration;
			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}
	}
}
