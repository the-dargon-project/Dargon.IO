using Dargon.IO.Components;
using Dargon.Ryu;
using ItzWarty;
using ItzWarty.IO;
using ItzWarty.Networking;
using NMockito;
using Xunit;

namespace Dargon.IO.Drive {
   public class DargonNodeFactoryDriveExtensionsTests : NMockitoInstance {
      [Mock] private readonly IStreamFactory streamFactory = null;
      [Mock] private readonly DataStreamComponentFactory dataStreamComponentFactory = null;
      [Mock] private readonly DriveTreeImporter importer = null;

      [Mock] private readonly DargonNodeFactory nodeFactory = null;

      public DargonNodeFactoryDriveExtensionsTests() {
         var ryu = CreateMock<RyuContainer>();
         When(ryu.Get<IStreamFactory>()).ThenReturn(streamFactory);
         When(ryu.Get<DataStreamComponentFactory>()).ThenReturn(dataStreamComponentFactory);
         When(ryu.Get<DriveTreeImporter>()).ThenReturn(importer);
         DargonNodeFactoryDriveExtensions.Initialize(ryu);
         ClearInteractions();
      }

      [Fact]
      public void CreateDriveDirectoryNode_DelegatesToFactory_Test() {
         var node = CreateMock<WritableDargonNode>();
         var kDirectoryName = CreatePlaceholder<string>();

         When(nodeFactory.Create(kDirectoryName)).ThenReturn(node);

         var result = nodeFactory.CreateDriveDirectoryNode(kDirectoryName);

         Verify(nodeFactory).Create(kDirectoryName);
         VerifyNoMoreInteractions();

         AssertEquals(node, result);
      }

      [Fact]
      public void CreateDriveFileNode_DelegatesToFactoryAndRegisteredDataStream_Test() {
         var fileName = CreatePlaceholder<string>();
         var filePath = CreatePlaceholder<string>();
         var fileInfo = CreateMock<IFileInfo>();
         var node = CreateMock<WritableDargonNode>();
         var dataStreamComponent = CreateMock<DataStreamComponent>();

         When(fileInfo.Name).ThenReturn(fileName);
         When(fileInfo.FullName).ThenReturn(filePath);
         When(nodeFactory.Create(fileName)).ThenReturn(node);
         When(dataStreamComponentFactory.CreateForFile(filePath)).ThenReturn(dataStreamComponent);

         var result = nodeFactory.CreateDriveFileNode(fileInfo);

         Verify(fileInfo).Name.Wrap();
         Verify(fileInfo).FullName.Wrap();
         Verify(nodeFactory).Create(fileName);
         Verify(node).AddComponent(dataStreamComponent);
         Verify(dataStreamComponentFactory).CreateForFile(filePath);
         VerifyNoMoreInteractions();

         AssertEquals(node, result);
      }

      [Fact]
      public void ImportDirectoryAndParents_DelegatesToImporter_Test() {
         var directoryPath = CreatePlaceholder<string>();
         var expectedResult = CreateMock<WritableDargonNode>();

         When(importer.ImportDirectoryAndParents(nodeFactory, directoryPath)).ThenReturn(expectedResult);

         var actualResult = nodeFactory.ImportDirectoryAndParents(directoryPath);

         Verify(importer).ImportDirectoryAndParents(nodeFactory, directoryPath);
         VerifyNoMoreInteractions();

         AssertEquals(expectedResult, actualResult);
      }

      [Fact]
      public void ImportFileTree_DelegatesToImporter_Test() {
         var directoryPath = CreatePlaceholder<string>();
         var expectedResult = CreateMock<WritableDargonNode>();

         When(importer.ImportFileTree(nodeFactory, directoryPath)).ThenReturn(expectedResult);

         var actualResult = nodeFactory.ImportFileTree(directoryPath);

         Verify(importer).ImportFileTree(nodeFactory, directoryPath);
         VerifyNoMoreInteractions();

         AssertEquals(expectedResult, actualResult);
      }
   }
}