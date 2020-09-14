using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CrmEarlyBound;
using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using UDS.VoPlugin;
using UDS.VoPlugin.Repository;

namespace ConsoleCRMApp
{
    class Program
    {
        static void Main(string[] args)
        {
           
            OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
            var service = (IOrganizationService)serviceProxy;
            serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());
            //VoService voService = new VoService(service);
            //voService.Deactivate();
            //voService.EarlyBoundDeactivate();

            ///////////////////////////////////////////////////////////////////////////////////////////////////

            //CrmServiceContext context = new CrmServiceContext(service);
            //VoMainScriptDeactivateRepository voMainScript = new VoMainScriptDeactivateRepository(service);
            //var e = voMainScript.EarlyBoundGetEntities(new Guid("E71698A5-1BEA-EA11-8129-00155D06F203"));


            #region Task14Testing

            TempRepository tempRepository = new TempRepository(service);
            var vo = tempRepository.GetVoMainScriptRecors().Entities;
            foreach (var item in vo)
            {
                Console.WriteLine(item.Id+" "+item.Attributes["new_name"]);
            }
            var entity = vo[0];
            var name = entity.LogicalName;
            var id = entity.Id;

            string path = @"D:\C# Junior собеседование\UDS Consulting (стажировка)\Developer Education\ЛК1.docx";
            string fileAsString = GetBase64StringFromFile(path);
            string fileName = Path.GetFileName(path);
            string mimeType = MimeMapping.GetMimeMapping(fileName);

            Entity Note = new Entity("annotation");
            Note["objectid"] = new EntityReference(name, id);
            Note["objecttypecode"] = name;
            Note["subject"] = "Test Subject";
            Note["notetext"] = "Test note text";

            Note["documentbody"] = fileAsString;
            Note["mimetype"] = mimeType;
            Note["filename"] = fileName;

            service.Create(Note);
#endregion

            Console.ReadLine();

        }
        private void AttachUpdatedDocumentToCase(ref IOrganizationService service, byte[] filebytes, EntityReference _CaseReference, string FileName)
        {
            string entitytype = "incident";
            Guid EntityToAttachTo = Guid.Parse("FDA93807-4EF3-E711-80F2-3863BB2E34E8"); // The GUID of the incident

            Entity Note = new Entity("annotation");
            Note["objectid"] = new Microsoft.Xrm.Sdk.EntityReference(entitytype, EntityToAttachTo);
            Note["objecttypecode"] = entitytype;
            Note["subject"] = "Test Subject";
            Note["notetext"] = "Test note text";
            service.Create(Note);



            //string encodedData = System.Convert.ToBase64String(filebytes);
            //Entity AnnotationEntityObject = new Entity(_AnnotationEntityName);
            //AnnotationEntityObject.Attributes[_ObjectIdFieldName] = _CaseReference;
            //AnnotationEntityObject.Attributes[_SubjectFieldName] = FileName;
            //AnnotationEntityObject.Attributes[_DocumentBodyFieldName] = encodedData;
            //// Set the type of attachment
            //AnnotationEntityObject.Attributes[_MimeTypeFieldName] = @"application\pdf";
            //// Set the File Name
            //AnnotationEntityObject.Attributes[_FileNameFieldName] = FileName;
            //service.Create(AnnotationEntityObject);
        }
        static string GetBase64StringFromFile(string path)
        {
            string res = "";
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);

                res = Convert.ToBase64String(array);
            }
            return res;
        }

    }

}
