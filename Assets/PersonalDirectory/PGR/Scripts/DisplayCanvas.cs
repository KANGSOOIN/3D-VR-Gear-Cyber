namespace PGR
{
    /// <summary>
    /// ȫä ���÷��̸� Ȱ��ȭ���� �� ���̴� World Space UI
    /// ��ġ�� ������ �����ϰ�, �ؽ�Ʈ�� �Է��ϸ� ��
    /// </summary>
    public class DisplayCanvas : SceneUI
    {
        public void ChangeMainText(string context)
        {
            texts["MainText"].text = context;
        }

        public void ChangeSubText(string context)
        {
            texts["SubText"].text = context;
        }
    }

}