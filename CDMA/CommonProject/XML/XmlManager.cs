using System.IO;
using System.Xml.Linq;

namespace CommonProject
{
    public class XmlManager
    {
        public static void XmlLoad(XDocSaveOrLoad xDocLoad)
        {
            switch (xDocLoad)
            {
                case XDocSaveOrLoad.User:
                    XmlData.XDocUser = XDocument.Load(XmlData.XmlSavePath + XmlData.xmlFileNameUser);
                    break;

                case XDocSaveOrLoad.Food:
                    XmlData.XDocFood = XDocument.Load(XmlData.XmlSavePath + XmlData.xmlFileNameFood);
                    break;

                case XDocSaveOrLoad.Supplement:
                    XmlData.XDocSupplement = XDocument.Load(XmlData.XmlSavePath + XmlData.xmlFileNameSupplement);
                    break;
            }
        }
        public static void XmlSave(XDocSaveOrLoad xDocSave)
        {
            switch (xDocSave)
            {
                case XDocSaveOrLoad.User:
                    XmlData.XDocUser.Save(XmlData.XmlSavePath + XmlData.xmlFileNameUser);
                    break;

                case XDocSaveOrLoad.Food:
                    XmlData.XDocFood.Save(XmlData.XmlSavePath + XmlData.xmlFileNameFood);
                    break;

                case XDocSaveOrLoad.Supplement:
                    XmlData.XDocSupplement.Save(XmlData.XmlSavePath + XmlData.xmlFileNameSupplement);
                    break;
            }
            XmlLoad(xDocSave);
        }
        public static void XmlUserControl()
        {
            if (File.Exists(XmlData.XmlSavePath + XmlData.xmlFileNameUser))
                XmlLoad(XDocSaveOrLoad.User);
            else
                XmlHelpMethod.CreateNewXDocument(XDocSaveOrLoad.User);
        }
        public static void XmlFoodControl()
        {
            if (File.Exists(XmlData.XmlSavePath + XmlData.xmlFileNameFood))
                XmlLoad(XDocSaveOrLoad.Food);
            else
                XmlHelpMethod.CreateNewXDocument(XDocSaveOrLoad.Food);
        }
        public static void XmlSupplementControl()
        {
            if (File.Exists(XmlData.XmlSavePath + XmlData.xmlFileNameSupplement))
                XmlLoad(XDocSaveOrLoad.Supplement);
            else
                XmlHelpMethod.CreateNewXDocument(XDocSaveOrLoad.Supplement);
        }
    }
}
