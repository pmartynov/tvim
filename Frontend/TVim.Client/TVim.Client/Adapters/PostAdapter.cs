using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace TVim.Client.Activity
{
    public class PostAdapter : RecyclerView.Adapter
    {
        protected readonly Context Context;
        private List<Post> _items;

        public override int ItemCount => _items.Count;

        public PostAdapter(Context context, List<Post> items)
        {
            Context = context;
            _items = items;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var post = _items[position];
            if (post == null)
                return;
            var vh = (FeedViewHolder)holder;
            vh.UpdateData(post, Context);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.lyt_post_item, parent, false);
            var vh = new FeedViewHolder(itemView);
            return vh;
        }

        public class FeedViewHolder : RecyclerView.ViewHolder
        {
            protected readonly Context Context;

            private readonly ImageView _image;

            protected Post Post;

            public FeedViewHolder(View itemView) : base(itemView)
            {
                Context = itemView.Context;
                _image = itemView.FindViewById<ImageView>(Resource.Id.image);

            }

            public void UpdateData(Post post, Context context)
            {
                Post = post;
                Picasso.With(Context).Load(Post.Url).Into(_image);
            }
        }
    }
}
