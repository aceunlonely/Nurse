V1.0.0:

业务心跳开发：
	引入对应CPU架构的Nurse.Common.dll
	业务代码关键逻辑处添加 BeatManger.Beat()

业务心跳部署配置：
	appsetting中添加节点
		WebStateCenterUrl  ： stateCenter的url
		BeatAppName ： 服务名 ，或者 EXE名
	请确保服务器存在D盘， 如果d盘不存在，请配置DiskStateCenterPath

WebStateCenter 部署说明：
	直接部署
	map.config 配置： (用于映射服务在监控页面上的显示名称，详细参考map.config.demo)
		节点Code 对应监控中的服务名
		节点Name 对应需要在index.aspx页面中显示的服务中文名称
	验证URL：
		http://localhost/WebStateCenter/index.aspx
		http://localhost/WebStateCenter/main.ashx
Nurse.Master.Service 部署说明：
	appsetting中添加节点 (即Nurse.Master.Service.exe.config中配置)
		IsAlwaysRun ： 是否一直运行， 如果开启，就master和slave相互守护，不能被终止
		Internal ： 通用间隔
		Internal_Check_Slave ： master 检查slave的间隔 （默认值就是Internal）
		WebStateCenterUrl  ： WebStateCenter的url
	请确保服务器存在D盘， 如果d盘不存在，请配置DiskStateCenterPath
	nurse.config添加配置，用于配置监控的服务及处理策略（根据实际配置,配置说明参考nurse.config.demo）
	mq.config 添加配置，用于mq通道深度监控（根据实际配置，配置说明参考mq.config.demo
		其中ConfigDomain/Name 为主机配置别名，随意起名
		ConfigDomain/Value为“192.168.10.228|WORKGROUP|administrator|dcjet@888”的加密字符串
					加密方式采用apollo的密码工具：PasswordTool.exe
		MSMQConfigNode/Instance 为msmq的实例名，参考格式：highvertest\private$\tx1
		MSMQConfigNode/Domain 对应ConfigDomain/Name，空值时，表明是本机的mq服务

	服务设置为开机自动启动


运维使用说明：
	1、被监控服务更新发版时：
		Nurse.Master.Service 服务需要停掉，停止时注意可能需要停多次（请确保配置项IsAlwaysRun=false）
		更新被监控服务
		再启动Nurse.Master.Service服务
	2、被监控服务停止时：
		修改nurse.config、mq.config配置（可以去掉节点，也能继续跟踪，配置为不处理）
		重启Nurse.Master.Service（建议先停止，后开启，确保重启）
		停止被监控服务
	3、新增被监控服务
		正常部署安装服务（服务如需要心跳监控，需要特别开发）
		修改Nurse的配置 nurse.config
		重启Nurse.Master.Service（建议先停止，后开启，确保重启）、
	4、新增通道监控：
		直接配置Nurse.Master.Service的mq.config
		重启服务
	5、更新WebStateCenter：
		直接更新
	6、更新Nurse.Master.Service：
		停止服务，请确保配置项IsAlwaysRun=false，确保停止后，更新文件


V1.1.0
    v1.0.0基础上添加配置
    WebStateCenter 部署说明：
        根目录添加 mq.config配置项，详细配置见 mq.config.demo
    需要进行监控的master.service 需要配置节点 mq.config

    
V1.2.0
	yyjk心跳监控开发 如V1.0.0: 业务心跳开发：
	部署：
		appsetting中添加节点
			BeatAppName			：服务名 ，或者 EXE名 （随意）
			YYJK_WebapiUrl		：yyjk webapi的地址
			YYJK_Provider		：yyjk 应用提供者
			YYJK_SystemCode		：yyjk配置的系统code
			YYJK_MachineCode	：yyjk配置的机器标识
			YYJK_CollectItemKey	：yyjk配置的采集项名  默认beat
			YYJK_HostIp			：yyjk配置中的hostip  默认是本机hostip