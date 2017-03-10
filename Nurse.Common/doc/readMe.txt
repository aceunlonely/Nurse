业务心跳开发：
	业务代码关键逻辑处添加 BeatManger.Beat()

业务心跳部署配置：

	appsetting中添加节点
		WebStateCenterUrl  ： stateCenter的url
		BeatAppName ： 服务名 ，或者 EXE名
	请确保服务器存在D盘， 如果d盘不存在，请配置DiskStateCenterPath

WebStateCenter 部署说明：
	直接部署


Nurse.Master.Service 部署说明：
	appsetting中添加节点
		IsAlwaysRun ： 是否一直运行， 如果开启，就master和slave相互守护
		Internal ： 通用间隔
		Internal_Check_Slave ： master 检查slave的间隔 （默认值就是Internal）
		WebStateCenterUrl  ： stateCenter的url

	请确保服务器存在D盘， 如果d盘不存在，请配置DiskStateCenterPath
	nurse.config添加配置（根据实际配置,配置说明参考nurse.config.demo）

	设置为开机启动