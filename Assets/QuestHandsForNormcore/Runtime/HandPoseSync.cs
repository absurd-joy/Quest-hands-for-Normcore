using Normal.Realtime;

namespace absurdjoy
{
	public class HandPoseSync : RealtimeComponent<GenericStringModel>
	{
		private QuestSkeletonSerializer questSkeletonSerializer;

		private void OnEnable()
		{
			questSkeletonSerializer = GetComponent<QuestSkeletonSerializer>();
		}

		protected override void OnRealtimeModelReplaced(GenericStringModel previousModel, GenericStringModel currentModel)
		{
			base.OnRealtimeModelReplaced(previousModel, currentModel);
			if (previousModel != null)
			{
				previousModel.stringValueDidChange -= ReceivedData;
			}

			if (currentModel != null)
			{
				if (!currentModel.isFreshModel)
				{
					ReceivedData(currentModel, model.stringValue);
				}

				currentModel.stringValueDidChange += ReceivedData;
			}
		}

		private void ReceivedData(GenericStringModel model, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}

			questSkeletonSerializer.DeserializeSkeletalData(value);
		}

		public void SendData(string data)
		{
			model.stringValue = data;
		}
	}
}