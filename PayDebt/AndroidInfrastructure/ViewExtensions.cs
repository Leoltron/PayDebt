﻿using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace PayDebt.AndroidInfrastructure
{
    public static class ViewExtensions
    {
        public static void SetNormalTextForTextView(this View view, int resId, string value)
        {
            view.FindViewById<TextView>(resId).SetText(value, TextView.BufferType.Normal);
        }

        public static void AddPaintFlag(this TextView textView, PaintFlags flag)
        {
            textView.PaintFlags = textView.PaintFlags | flag;
        }

        public static void RemovePaintFlag(this TextView textView, PaintFlags flag)
        {
            textView.PaintFlags = textView.PaintFlags & ~flag;
        }

        public static void FormatText(this TextView textView, params object[] args)
        {
            textView.Text = string.Format(textView.Text, args);
        }
    }
}