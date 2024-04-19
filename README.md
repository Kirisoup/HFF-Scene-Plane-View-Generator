# ScPVGen

Generates a 7680^2 sized top down isometric view png of the level

You can press M to turn on the enlarged preview.

Once the preview is toggled on, you can use use the arrow keys to move the center arround (up key moves it in the +Z direction, right key moves it in the +X direction). By default it moves the center by 1.0 meter on each key press, 0.5 if holding shift, 10.0 if holding control; hold alt to keep moving it as long as the arrow keys are held down.

You can also use the scroll wheel to change the range of the view area (default 1.0 unit per scroll, 0.5 if holding shift, and 10 if holding control); hold alt while scrolling to change the cut-off height (mountains would be cut open with a lower cut-off height for example).

Once you are satisfied with the preview, you can save it with ctrl+S.

## Note: 

Sometimes when generating too many imags (or if the rendered scene is too large), the image generated will simply be a 720kb blank png, or your game might crash (might be my spaghetti code issue). 

In most of the times you'd need to reboot the game, but in Port especially, you would very likely need to turn down the resolution if you want to render the entire scene (including the wide ocean). 

Well, currently there is no way of doing so without changing the source code, sorry. I'm just not good enough to just come up with elegant and well-polished code, an this is not meant to be a project which I put a lot of efforts in to.
