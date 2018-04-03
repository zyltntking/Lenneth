using System.Reflection;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection
{
    public static class EmbeddedHaarCascades
    {
        private static HaarCascade.HaarCascade _frontFaceDefault;

        public static HaarCascade.HaarCascade FrontFaceDefault
        {
            get
            {
                return _frontFaceDefault ?? (_frontFaceDefault = GetCascadeFromResource("haarcascade_frontalface_legacy.xml"));
            }
        }

        private static HaarCascade.HaarCascade GetCascadeFromResource(string identifier)
        {
            HaarCascade.HaarCascade cascade;
            var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("ImageProcessor.Imaging.Filters.ObjectDetection.Resources." + identifier);

            //using (StringReader stringReader = new StringReader(resource))
            //{
                cascade = HaarCascade.HaarCascade.FromXml(resource);
            //}

            return cascade;
        }
    }
}
