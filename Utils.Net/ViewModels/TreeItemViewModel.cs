﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Utils.Net.Common;
using Utils.Net.Extensions;
using Utils.Net.Helpers;

namespace Utils.Net.ViewModels
{
    /// <summary>
    /// Abstract ViewModel for a tree item.
    /// </summary>
    public abstract class TreeItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets parent item of the current.
        /// </summary>
        public TreeItemViewModel ParentItem { get; }

        private bool isExpanded;
        /// <summary>
        /// Gets or sets a value indicating whether the item is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (SetPropertyBackingField(ref isExpanded, value))
                {
                    IsExpandedChanged?.Invoke(this, isExpanded.ToEventArgs());
                }
            }
        }

        /// <summary>
        /// Occurs when the <see cref="IsExpanded"/> property get changed.
        /// </summary>
        public event EventHandler<EventArgs<bool>> IsExpandedChanged;

        /// <summary>
        /// Gets the <see cref="ObservableCollection{TreeItemViewModel}"/> of the children items.
        /// </summary>
        public ObservableCollection<TreeItemViewModel> Children { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItemViewModel"/> class.
        /// </summary>
        /// <param name="parentItem">Parent item of the current.</param>
        /// <param name="isExpanded">Indicate whether the item is expanded; default false.</param>
        public TreeItemViewModel(TreeItemViewModel parentItem = null, bool isExpanded = false)
        {
            ParentItem = parentItem;
            this.isExpanded = isExpanded;
            Children = new ObservableCollection<TreeItemViewModel>();
        }
    }

    /// <summary>
    /// Common ViewModel for a tree item with generic content.
    /// </summary>
    /// <typeparam name="T">Type of the item's content.</typeparam>
    public class TreeItemViewModel<T> : TreeItemViewModel
    {
        /// <summary>
        /// Gets the content of the current item.
        /// </summary>
        public T Content { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItemViewModel{T}"/> class.
        /// </summary>
        /// <param name="content">Content of the item.</param>
        /// <param name="parentItem">Parent item of the current.</param>
        /// <param name="isExpanded">Indicate whether the item is expanded; default false.</param>
        public TreeItemViewModel(T content, TreeItemViewModel parentItem = null, bool isExpanded = false)
            : base(parentItem, isExpanded)
        {
            Content = content;
        }


        /// <summary>
        /// Execute <see cref="Action{T}"/> and propagate it through the children.
        /// </summary>
        /// <param name="action"><see cref="Action{T}"/> that will be propagated.</param>
        public void Propagate(Action<TreeItemViewModel<T>> action)
        {
            action.Invoke(this);
            Children.OfType<TreeItemViewModel<T>>().ForEach(c => c.Propagate(action));
        }

        /// <summary>
        /// Overrides the default ToString method. 
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            string res = string.Empty;
            if (ParentItem != null)
            {
                res = $"{ParentItem}.";
            }
            res += $"{(IsExpanded ? "-" : "+")}{Content}";
            return res;
        }
    }
}
