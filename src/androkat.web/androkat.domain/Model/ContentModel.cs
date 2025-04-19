namespace androkat.domain.Model;

public class ContentModel
{
    public ContentModel(ContentDetailsModel contentDetails, ContentMetaDataModel metaData)
    {
        ContentDetails = contentDetails;
        MetaData = metaData;
    }

    public ContentDetailsModel ContentDetails { get; }
    public ContentMetaDataModel MetaData { get; }
}