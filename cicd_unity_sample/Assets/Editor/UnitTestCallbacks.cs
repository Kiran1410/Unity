
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
class UnitTestCallbacks : ICallbacks
{

    private static readonly string TAG = "MyCallbacks";

    public UnitTestCallbacks()
    {
    }

    public void RunFinished(ITestResultAdaptor result)
    {
      //  LocalLogger.ShowLocalLog("RunFinished: Report Xml " + result.ToXml().OuterXml, TAG);
        Debug.Log("RunFinished: Report Xml " + result.ToXml().OuterXml);
        UnityTestReportGenerator.CreateXmlReport(result, ABConstants.XML_PATH);
    }

    public void RunStarted(ITestAdaptor testsToRun)
    {
        //LocalLogger.ShowLocalLog("RunStarted: count of tests : " + testsToRun.Children, TAG);
       Debug.Log("RunStarted: count of tests : " + testsToRun.Children);
    }

    public void TestFinished(ITestResultAdaptor result)
    {

        Debug.Log("TestFinished: test name : " + result.Name);
        if (!result.HasChildren && result.ResultState == "Passed")
        {
            //LocalLogger.ShowLocalLog("TestFinished: result : passed" + result.Test.Name, TAG);
        }
        else
        {
            //LocalLogger.ShowLocalLog("TestFinished: result : Fail" + result.Test.Name, TAG);
        }
    }

    public void TestStarted(ITestAdaptor test)
    {
        //LocalLogger.ShowLocalLog("TestStarted: test name : " + test.Name, TAG);
        Debug.Log("TestStarted: test name : " + test.Name);
    }

}