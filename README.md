# EXEManager

# 셋팅

```csharp
Dictionary<string, EXEManager.S3D.Command> commandBundle = new Dictionary<string, EXEManager.S3D.Command>();
commandBundle.Add("SYSTEM_PATH", GetSystemPath);
commandBundle.Add("GET_REV_ITEM", GetRevisionItem);
commandBundle.Add("S3D_DRAWING", DrawingItem);
commandBundle.Add("GET_PART_DATA_FDN", GetPartFDN);
commandBundle.Add("GET_PART_DATA_UD", GetPartUD);
commandBundle.Add("ZOOM_TO_OBJECT", ZoomToObject);

EXES3DCommander s3dCommander = new EXES3DCommander("S3D_COMMAND", commandBundle);
```

다음 구조를 가짐

- EXES3DCommander : EXECommander
- EXERevitCommander : EXECommander
- EXETeklaCommander : EXECommander
- EXEPDMSCommander : EXECommander

각각의 커멘더 클래스에 파일 저장 위치 및 레지스트리 키, 하이브키 위치 등 설정되어있음.

# 사용예시

```csharp
 public void GetRevisionItem(EXECommander e)
 {
     /// Cloud에서 다음과 같이 넘어와야함.
     // TABLE[0]: PEDAS OBJECT
     // TABLE[1]: SYSTEM_PATHID
     DataTable dt = new DataTable();
     dt.Columns.Add("PATH_NAME");
     ///////////////////////////////////

     var parameter = this.GetRevisionParamter(e.GetDataSetFromXML());

     var result = Revision.GetStructFootingSystem(parameter.Item1, parameter.Item2);

     // 결과 반환
     e.SetXMLByDataSet(result.DataSet);
     e.SetRegistryValue("GET_REV_ITEM_COMPLETE");
     
     
     

		//보내고 싶은 리비전 리스트
		List<IRevisionInfo> revision_SEND = new List<IRevisionInfo>();
		
		//여기서 이렇게 주고싶은 리비전 셋팅하면
		e.SetXMLByCLassInstance(revision_SEND);
		
		//UI 에서 이렇게 받을 수 있음.
		List<IRevisionInfo> revision_GET = e.GetXMLByClassInstance<List<IRevisionInfo>>(); 
		

 }
```

# EXECommander e

| 메서드 | Parameter | 비고 |
| --- | --- | --- |
| e.CreateDirectory() | void | 디렉터리 생성 (위치는 자동) |
| e.SetXMLByDataSet() | DataSet | DataSet을 XML 로 셋팅 (위치는 자동) |
| e.GetDataSetFromXML() | void | 셋팅한 DataSet을 가져옴 (위치는 자동) |
| e.SetXMLByCLassInstance<T>() | T | 클래스 인스턴스를 셋팅 (위치는 자동) |
| e.GetXMLByClassInstance<T>() | T | 셋팅된 클래스 인스턴스를 가져옴 (위치는 자동) |
| e.SetRegistryValue() | string | 레지스트 “값” 변경 (위치는 자동) |
| e.GetRegistryValue() | void | 레지스트 “값” 가져오기 (위치는 자동) |