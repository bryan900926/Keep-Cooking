### complete part
客人生成時門會打開，隨後關上
### modified part
#### DoorController.cs(create)
- overview:
  - 屬性/方法,說明
      - doorAnimator,門物件上的 Animator 元件的緩存引用。
      - openTriggerName,"定義 Animator 中用於開門的 Trigger 參數名稱，默認為 ""Open""。"
      - reopenCooldown,設定防止動畫被快速連續觸發的冷卻時間（默認為 0.25 秒），提高運行穩定性。
      - Awake(),確保在組件初始化時獲取 Animator 元件。
      - TriggerDoorOpen(),公開介面。供 CustomerSpawner 呼叫，負責：1. 檢查冷卻時間。2. 確保 Animator 不會在播放動畫時被重複設定 Trigger (ResetTrigger)。3. 使用 SetTrigger(openTriggerName) 啟動開門動畫。
#### CustomerSpawner.cs(modified)
- overview:
  - 修改點,說明
      - 新增欄位,引入 [SerializeField] private DoorController doorController; 欄位，用於在Inspector 中連結到門物件上的控制器。
      - SpawnCustomer(),在生成客人之前，呼叫了 TriggerDoorOpen() 助手方法。
      - SpecialSpawner(),在生成特殊客人之前，同樣呼叫了 TriggerDoorOpen() 助手方法。
      - 新增助手方法,引入私有方法 private void TriggerDoorOpen()，統一管理對 doorController?.TriggerDoorOpen() 的調用。
#### unity editoer part:
- open_door.png added
- Door GameObject:
  - 附加了 DoorController.cs 腳本
  - Animator： 確保門物件或其子物件上附加了 Animator 元件，並將其連結到 DoorController 腳本的 Door Animator 欄位
  - Trigger： DoorController 腳本中的 Open Trigger Name 欄位必須設定為 Open
- Animator state machine:
  - 元素,設定,目的
      - Parameters,建立 Trigger 參數，名稱為 Open。,程式碼用來觸發動畫的指令名稱。
      - Idle 狀態,設定為 Default State。Motion 建議為 None（門物件的 Sprite Renderer 顯示關門圖片）。,門的預設狀態（關閉）。
      - Idle → door_open,Conditions: 設定為 Trigger: Open。Has Exit Time: 取消勾選。,確保門在 CustomerSpawner 呼叫時立即開門。
      - door_open → Idle,Conditions: 保持為空。Has Exit Time: 勾選。,確保開門動畫播放完畢後，門會自動切換回關閉的 Idle 狀態。