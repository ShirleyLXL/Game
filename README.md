# Game
# TODO
## 添加好友功能
  - 需要发送消息给后端数据库查看此Player是否存在
  - 后端发给前端消息，前端将好友信息显示在UI上
## 消息离线发送
  - 数据库处理消息时(OnRecvChatMessage)，如果对应的receiver不在线，将消息存到数据库中
  - 需要修改friendlist加载时，数据库中如果有聊天信息，则需要加载出来QAQ，消息条数提示。。。
  - 还有个问题就是，在receivefriendMessage时候，当前窗口不是对应的friend甚至！没有打开friendlist的时候？（这时候好友是在线的！）
    - 可能的解决：在wechat的图标上也放上消息条数提示，点击时每次检查这个消息条数提示，如果有消息的话，和friendlist一起加载（可以考虑把消息一起存到内存）；点击对应的friend时候发送消息给后端将离线聊天消息取出(或者只是发送消息给后端将聊天消息删除）
