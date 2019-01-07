﻿namespace Xamarin.Forms.Platform.AvaloniaUI.Interfaces
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Threading;

    public interface IContentLoader
	{
        Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken);

        void OnSizeContentChanged(Control parent, object content);
    }

	public class DefaultContentLoader : IContentLoader
	{
		public Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken)
		{
            if (!Dispatcher.UIThread.CheckAccess())
            {
                throw new InvalidOperationException("UIThreadRequired");
            }

            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			return Task.Factory.StartNew(() => LoadContent(newContent), cancellationToken, TaskCreationOptions.None, scheduler);
		}

		protected virtual object LoadContent(object content)
		{
			if (content is Control)
				return content;
   
            // TODO: 
            //if (content is Uri)
            //    return Application.LoadComponent(content as Uri);

            //if (content is string)
            //{
            //    if (Uri.TryCreate(content as string, UriKind.RelativeOrAbsolute, out Uri uri))
            //    {
            //        return Application.LoadComponent(uri);
            //    }
            //}

            return null;
		}

		public void OnSizeContentChanged(Control parent, object page)
		{

		}
	}
}