namespace androkat.web.Endpoints.RadioMusorModelEndpoints;

public class RadioMusorModelResponse
{
	public RadioMusorModelResponse(bool result)
	{
		Result = result;
	}

	public bool Result { get; init; }
}