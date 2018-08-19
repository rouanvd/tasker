module Webfu.DOM.Notification --Export List
( mkNotification
, mkNotification'
, NotificationOpts
, notificationOpts
, Notification
, module Webfu.DOM.Lang
, lang
, permission
, body'
, dir 
, tag
, icon
, title
) where

import Prelude
import Webfu.DOM.Lang

import Data.Function.Uncurried (Fn0, Fn1, Fn10, Fn2, Fn3, runFn0, runFn1, runFn2, runFn3)
import Prelude (Unit, unit)
import Effect (Effect)
import Data.Function.Uncurried
import Data.Maybe (Maybe)
import Effect (Effect)
import Foreign (Foreign)
import Webfu.DOM.Promise (Promise)
import Webfu.Data.Err (Err(..))
import Webfu.Data.ObjMap (Obj, Options, Option, options, (:=), empty)
import Webfu.Interop

foreign import data Notification :: Type

foreign import mkNotificationImpl :: Fn2 String Obj Notification

--Works
mkNotification :: String -> Notification
mkNotification s = runFn2 mkNotificationImpl s empty

--Works
mkNotification' :: String -> Options -> Notification
mkNotification' s opts = runFn2 mkNotificationImpl s (options opts)

--Works
foreign import permissionImpl :: Fn0 String
permission :: Unit -> String
permission _ = runFn0 permissionImpl

--Works
lang :: Notification -> String 
lang n = readString "lang" $ toJsObject n 

--Works
body' :: Notification -> String
body' n = readString "body" $ toJsObject n 

--Works
dir :: Notification -> String 
dir n = readString "dir" $ toJsObject n 

--Works
tag :: Notification -> String 
tag n = readString "tag" $ toJsObject n

--Works
icon :: Notification -> String 
icon n = readString "icon" $ toJsObject n

--Works
title :: Notification -> String 
title n = readString "title" $ toJsObject n

-- --Works
foreign import requestPermissionImpl :: Fn0 (Promise String Err)
requestPermission :: Unit -> (Promise String Err)
requestPermission _ = runFn0 requestPermissionImpl

--Works
foreign import closeImpl :: Fn2 Notification Int (Effect Unit)
close :: Notification -> Int -> (Effect Unit)
close n t = runFn2 closeImpl n t

type NotificationOpts = 
  { dir :: Option String
  , lang :: Option Lang 
  , badge :: Option String
  , body :: Option String 
  , tag :: Option String
  , icon :: Option String
  , image :: Option String
  }

notificationOpts :: NotificationOpts
notificationOpts = 
  { dir: ("dir" := _)
  , lang: \ v -> "lang" := show v
  , badge: ("badge" := _)
  , body: ("body" := _)
  , tag: ("tag" := _)
  , icon: ("icon" := _)
  , image: ("image" := _)
  }



 




 
