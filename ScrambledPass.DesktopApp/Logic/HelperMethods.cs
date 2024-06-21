namespace ScrambledPass.DesktopApp.Logic;

public static class HelperMethods
{
	public static int ParseIntSetting(string setting)
	{
		int value = 1;
		int.TryParse(Core.Instance.dataBank.GetSetting(setting), out value);
		return value;
	}
}