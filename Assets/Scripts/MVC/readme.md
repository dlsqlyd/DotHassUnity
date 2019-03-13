

Command 业务逻辑  指的是那些需要协调Model与View的逻辑。

Proxy 域逻辑 指的是仅仅是针对数据模型的操作，不论是对于客户端还是对于服务端，
不论是同步的操作还是异步的操作。后续的添加和修改接口只在Proxy中完成


DataObject 是完全对业务进行数据建模而产生的数据模型，与业务没有丝毫的关系


Mediator 添加事件监听，发送或接受Notification，改变组件状态等

ViewComponent只负责UI的绘制，而其他事情，包括事件的绑定统统交给Mediator来做
unity在这块大部分都是由prefab完成了..