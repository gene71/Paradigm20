using Paradigm.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Paradigm.Core
{
    public class ParaObjSerializer
    {
        /// <summary>
        /// SaveParaObj
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="crobj"></param>
        /// <exception cref="ParadigmException">ParadigmException</exception>
        public void SaveParaObj(string filename, object crobj)
        {
            try
            {
                using (var writer = new StreamWriter(filename))
                {
                    var serializer = new XmlSerializer(crobj.GetType());
                    serializer.Serialize(writer, crobj);
                    writer.Flush();

                }
            }
            catch (Exception ex)
            {
                throw new ParadigmException("error serializing ParaObj..." + ex.Message);
            }


        }
                

        /// <summary>
        /// LoadScanConfig
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        /// <exception cref="ParadigmException">ParadigmException</exception>
        public ScanConfig LoadScanConfig(string filepath)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open)) //double check that...
                {
                    XmlSerializer _xSer = new XmlSerializer(typeof(ScanConfig));

                    return (ScanConfig)_xSer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw new ParadigmException("error deserializing ScanConfig..." + ex.Message);
            }


        }

        /// <summary>
        /// ParadigmProject
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        /// <exception cref="ParadigmException">ParadigmException</exception>
        public ParadigmProject LoadProject(string filepath)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open)) //double check that...
                {
                    XmlSerializer _xSer = new XmlSerializer(typeof(ParadigmProject));

                    return (ParadigmProject)_xSer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw new ParadigmException("error deserializing ParadigmProject..." + ex.Message);
            }
        }

    }
}
