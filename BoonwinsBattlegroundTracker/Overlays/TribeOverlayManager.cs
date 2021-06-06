using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Hearthstone_Deck_Tracker;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace BoonwinsBattlegroundTracker
{
    public class TriverOverlayManager
    {
        private User32.MouseInput _mouseInput;
        private TribesOverlay _tribes;
        private Config _config;
        private Point mousePos0;
        private Point overlayPos0;
        private String _selected;

        public TriverOverlayManager(TribesOverlay tribesOverlay, Config c)
        {
            _tribes = tribesOverlay;
            _config = c;
                
        }

     

        public bool Toggle()
        {
            if (Hearthstone_Deck_Tracker.Core.Game.IsRunning && _mouseInput == null)
            {
                _mouseInput = new User32.MouseInput();
                _mouseInput.LmbDown += MouseInputOnLmbDown;
                _mouseInput.LmbUp += MouseInputOnLmbUp;
                _mouseInput.MouseMoved += MouseInputOnMouseMoved;
                return true;
            }
            Dispose();
            return false;
        }

        public void Dispose()
        {
            _mouseInput?.Dispose();
            _mouseInput = null;

        }

        private void MouseInputOnLmbDown(object sender, EventArgs eventArgs)
        {
            var position = User32.GetMousePos();
            mousePos0 = new Point(position.X, position.Y);
            overlayPos0 = new Point(_config.posLeft, _config.posTop);

            if (PointInsideControl(mousePos0, _tribes))
            {
                _selected = "tribes";
            }

            _config.save();
        }

        private void MouseInputOnLmbUp(object sender, EventArgs eventArgs)
        {
            var pos = User32.GetMousePos();


            if (_selected == "tribes")
            {
                _config.tribePosTop = overlayPos0.Y + (pos.Y - mousePos0.Y);
                _config.tribePosLeft = overlayPos0.X + (pos.X - mousePos0.X);
            }

            _selected = null;
            _config.save();
        }

        private void MouseInputOnMouseMoved(object sender, EventArgs eventArgs)
        {
            if (_selected == null)
            {
                return;
            }

            var pos = User32.GetMousePos();

  
            if (_selected == "tribes")
            {
                Canvas.SetTop(_tribes, overlayPos0.Y + (pos.Y - mousePos0.Y));
                Canvas.SetLeft(_tribes, overlayPos0.X + (pos.X - mousePos0.X));
            }

        }

        private bool PointInsideControl(Point p, FrameworkElement control)
        {
            try
            {
            var position = control.PointFromScreen(p);
            return position.X > 0 && position.X < control.ActualWidth && position.Y > 0 && position.Y < control.ActualHeight;
            }
            catch { return false; }
        }
    }
}

