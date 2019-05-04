using System.Xml.Linq;

namespace CommonProject
{
    class XmlHelpMethod
    {
        public static void CreateNewXDocument(XDocSaveOrLoad XDocType)
        {
            switch (XDocType)
            {
                case XDocSaveOrLoad.User:
                    XmlData.XDocUser = new XDocument();
                    XmlData.XDocUser.Add(new XElement(XmlData.XmlUserRootElement));
                    XmlManager.XmlSave(XDocSaveOrLoad.User);
                    break;

                case XDocSaveOrLoad.Food:
                    XmlData.XDocFood = new XDocument();
                    XmlData.XDocFood.Add(new XElement(XmlData.XmlFoodRootElement));
                    XmlManager.XmlSave(XDocSaveOrLoad.Food);
                    break;

                case XDocSaveOrLoad.Supplement:
                    XmlData.XDocSupplement = new XDocument();
                    XmlData.XDocSupplement.Add(new XElement(XmlData.XmlSupplementRootElement));
                    XmlManager.XmlSave(XDocSaveOrLoad.Supplement);
                    break;
            }
        }
    }
}
