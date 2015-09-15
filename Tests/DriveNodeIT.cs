using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Dargon.IO.Drive;
using ItzWarty.IO;
using NMockito;
using Xunit;

namespace Dargon.IO {
   public class DriveNodeIT : NMockitoInstance {
      private readonly IStreamFactory streamFactory;
      private readonly DargonNodeFactory dargonNodeFactory;

      public DriveNodeIT() {
         streamFactory = new StreamFactory();
         testObj = new DriveNodeFactory(streamFactory);
      }

      [Fact]
      public void Run() {
         var currentDirectory = Environment.CurrentDirectory;
         var testDataDirectory = Path.Combine(currentDirectory, "TestData");
         Debug.WriteLine("Expected Test Data Directory: " + testDataDirectory);
         var node = testObj.ImportFileTree(testDataDirectory);
         AssertEquals("TestData", node.Name);
         AssertEquals(2, node.Children.Count); // empty subdirectory is ignored
         var subdirectory1 = node.GetChild("Subdirectory1");
         AssertEquals(1, subdirectory1.Children.Count);
         var testFile = subdirectory1.GetChild("test");
         var testFileDataSource = testFile.GetComponentOrNull<IDataStreamComponent>();
         using (var testFileStream = testFileDataSource.GetDataStream()) 
         using (var testFileReader = testFileStream.GetReader()) {
            AssertEquals("test\r\nnewline", Encoding.UTF8.GetString(testFileReader.ReadAllBytes()));
         }

         var subdirectory2 = node.GetChild("Subdirectory2");
         AssertEquals(2, subdirectory2.Children.Count);
         foreach (var file in subdirectory2.Children) {
            var dataSource = file.GetComponentOrNull<IDataStreamComponent>();
            using (var stream = dataSource.GetDataStream()) 
            using (var reader = stream.GetReader()) {
               AssertEquals(file.Name, Encoding.UTF8.GetString(reader.ReadAllBytes()));
            }
         }
         
      }
   }
}
