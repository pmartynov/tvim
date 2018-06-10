using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
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
            private readonly Button _getPost;


            protected Post Post;

            public FeedViewHolder(View itemView) : base(itemView)
            {
                Context = itemView.Context;
                _image = itemView.FindViewById<ImageView>(Resource.Id.image);
                _getPost = itemView.FindViewById<Button>(Resource.Id.get_post);
                _getPost.Click += _getPost_Click;
            }

            private void _getPost_Click(object sender, EventArgs e)
            {
                var msg = $"LICENSE{System.Environment.NewLine}{System.Environment.NewLine}account: {Post.AccauntName}{System.Environment.NewLine}URL:{Post.Url}{System.Environment.NewLine}IPFS:{Post.IpfsHash}";
                var alert = new AlertDialog.Builder(Context);
                alert.SetMessage(msg);
                Dialog dialog = alert.Create();
                dialog.Show();
            }

            public void UpdateData(Post post, Context context)
            {
                Post = post;
                Picasso.With(Context).Load(Post.Url).Into(_image);
            }
        }

        public void Update(List<Post> posts)
        {
            _items = posts;
            NotifyDataSetChanged();
        }

        public void Add(Post post)
        {
            _items.Add(post);
            NotifyDataSetChanged();
        }

    }
}
