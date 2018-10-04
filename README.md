# fenestrate
Get all the weird off-screen app windows back onto monitors.

---

I have a four-monitor setup on my laptop docking station for a total of five screens. They're not all the same resolution. For some reason certain apps just love to come up and straddle two monitors in such a way that you can't easily move them but you also can't see the app. Sometimes child windows for things pop up off screen. **ARGH**

I know you can do, like, "Cascade Windows" but I don't want everything all moved around, I just want things back on screen. I also know you can do little hacky shortcuts like trying to use the window move commands to try to get things back on screen. The shortcuts didn't always work. I don't know why. I got tired of fighting with it.

I hacked this utility together so I could just run it and get stuff back on the screen. It's not great code, it's a lot of copy/paste from [pinvoke.net](http://pinvoke.net/), and probably some misunderstandings on my part of how low level window messaging gets done... **but it works for me.**

Maybe it'll work for you, too.

# Usage

Just run it. It goes. No command line parameters or anything.

# Known Issues

If you don't run it as admin it won't move administrator-based windows like Task Manager.