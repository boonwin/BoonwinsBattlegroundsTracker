using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Hearthstone_Deck_Tracker;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace BoonwinsBattlegroundTracker
{
    public class OverlayManager
    {

            private User32.MouseInput _mouseInput;
            private BgMatchOverlay _overlay;

            private Config _config;

            private Point mousePos0;
            private Point overlayPos0;

            private String _selected;

            public OverlayManager(BgMatchOverlay overlay, Config c)
            {
                _overlay = overlay;
                _config = c;
                UpdateConfig(c);
        }

        public void UpdateConfig(Config c)
        {


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
                var pos = User32.GetMousePos();
                mousePos0 = new Point(pos.X, pos.Y);
                overlayPos0 = new Point(_config.posLeft, _config.posTop);

                if (PointInsideControl(mousePos0, _overlay))
                {
                    _selected = "overlay";
                }

        

            _config.save();
        }

            private void MouseInputOnLmbUp(object sender, EventArgs eventArgs)
            {
                var pos = User32.GetMousePos();

                if (_selected == "overlay")
                {
                    _config.posTop = overlayPos0.Y + (pos.Y - mousePos0.Y);
                    _config.posLeft = overlayPos0.X + (pos.X - mousePos0.X);
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

                if (_selected == "overlay")
                {
                    Canvas.SetTop(_overlay, overlayPos0.Y + (pos.Y - mousePos0.Y));
                    Canvas.SetLeft(_overlay, overlayPos0.X + (pos.X - mousePos0.X));
                }

        }

            private bool PointInsideControl(Point p, FrameworkElement control)
            {
                var pos = control.PointFromScreen(p);
                return pos.X > 0 && pos.X < control.ActualWidth && pos.Y > 0 && pos.Y < control.ActualHeight;
            }
        
        }
    }


