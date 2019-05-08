﻿using Microsoft.AspNetCore.Components;
using System;

namespace Blazor20Questions.Client.Services
{
    public class ModalService
    {
        public event Action<string, RenderFragment> OnShow;
        public event Action OnClose;

        public void Show(string title, Type contentType)
        {
            var content = new RenderFragment(x => { x.OpenComponent(1, contentType); x.CloseComponent(); });
            OnShow?.Invoke(title, content);
        }

        public void Close()
        {
            OnClose?.Invoke();
        }
    }
}
