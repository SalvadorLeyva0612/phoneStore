﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;



namespace PhoneStore
{
    public partial class Default : System.Web.UI.Page
    {
        public string tipomenu = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] == null)
            {
                tipomenu = "0";
            }
            else
            {
                tipomenu = Request.QueryString["Id"];
            }
            TransformarXML();
        }
    private void TransformarXML()
    {
        //recuperamos las rutas de nuestros XML y XSLT
        string xmlPath = ConfigurationManager.AppSettings["FileServer"].ToString() + "xml/phoneStore.xml";
        string xsltPath = ConfigurationManager.AppSettings["FileServer"].ToString() + "xslt/template.xslt";

        //leer el archivo XML (en la parte de arriba, necesitamos los using de "System.Xml")
        XmlTextReader xmlTextReader = new XmlTextReader(xmlPath);

        //Configuramos las credenciales para resolver recursos extgernos como el XSLT
        XmlUrlResolver resolver = new XmlUrlResolver();
        resolver.Credentials = CredentialCache.DefaultCredentials;

        //creamos las configraciones para poder acceder al XSLT
        //los parámtetros "true" son para poder tratar el XSLT como si fuese un documento y así poder transformarlo
        XsltSettings settings = new XsltSettings(true, true);

        //leemos el archivos XSLT y lo cargamos para su transformación
        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load(xsltPath, settings, resolver);

        //Creamos un StringBuilder para almacenar el resultado de la transformación
        StringBuilder stringBuilder = new StringBuilder();
        TextWriter tWritter = new StringWriter(stringBuilder);

        //configramos los argumentos para la transformación del XSLT
        XsltArgumentList xsltArgumentList = new XsltArgumentList();
        //pasamos la variable del tipo menu al XSLT
        xsltArgumentList.AddParam("TipoMenu", "", tipomenu);

        //Transformamos el XML => HTML usando XSLT
        xslt.Transform(xmlTextReader, xsltArgumentList, tWritter);

        //Obtenemos el resultado de la traensformación como una sola cadena
        string resultado = stringBuilder.ToString();

        //Escribimos el resultado como una respuesta HTTP
        Response.Write(resultado);
    }
    }

}