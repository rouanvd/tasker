module Webfu.DOM.Fetch
  ( Response
  , responseOk
  , responseStatus
  , responseStatusText
  , responseBodyAsText
  , responseBodyAsJson
  , Headers
  , mkHeaders
  , headersAppend
  , fetchOpts
  , win_fetch
  , win_fetch'
  ) where

import Prelude (Unit)
import Data.Function.Uncurried (Fn0, runFn0, Fn1, runFn1, Fn3, runFn3)
import Foreign (Foreign)
import Effect (Effect)
import Webfu.Data.Err (Err(..))
import Webfu.Data.ObjMap (Obj, Options, Option, options, (:=), empty)
import Webfu.DOM.Core
import Webfu.DOM.Promise (Promise)


--------------------------------------------------------------------------------
-- Response
--------------------------------------------------------------------------------
foreign import data Response :: Type

foreign import responseOkImpl :: Fn1 Response Boolean
responseOk :: Response -> Boolean
responseOk r = runFn1 responseOkImpl r

foreign import responseStatusImpl :: Fn1 Response Int
responseStatus :: Response -> Int
responseStatus r = runFn1 responseStatusImpl r

foreign import responseStatusTextImpl :: Fn1 Response String
responseStatusText :: Response -> String
responseStatusText r = runFn1 responseStatusTextImpl r

foreign import responseBodyAsTextImpl :: Fn1 Response (Promise String Err)
responseBodyAsText :: Response -> Promise String Err
responseBodyAsText r = runFn1 responseBodyAsTextImpl r

foreign import responseBodyAsJsonImpl :: Fn1 Response (Promise Foreign Err)
responseBodyAsJson :: Response -> Promise Foreign Err
responseBodyAsJson r = runFn1 responseBodyAsJsonImpl r


--------------------------------------------------------------------------------
-- Headers
--------------------------------------------------------------------------------
foreign import data Headers :: Type

foreign import mkHeadersImpl :: Fn0 Headers
mkHeaders :: Unit -> Headers
mkHeaders _ = runFn0 mkHeadersImpl

foreign import headersAppendImpl :: Fn3 String String Headers Headers
headersAppend :: String -> String -> Headers -> Headers
headersAppend key value headers = runFn3 headersAppendImpl key value headers


--------------------------------------------------------------------------------
-- Fetch
--------------------------------------------------------------------------------

type FetchOpts =
  { method         :: Option String
  , headers        :: Option Headers
  , body           :: Option String
  , mode           :: Option String
  , credentials    :: Option String
  , cache          :: Option String
  , redirect       :: Option String
  , referrer       :: Option String
  , referrerPolicy :: Option String
  , integrity      :: Option String
  , keepAlive      :: Option String
  --, signal         :: Option String   -- not implemented yet
  }

fetchOpts :: FetchOpts
fetchOpts =
  { method: ("method" := _)
  , headers: ("headers" := _)
  , body: ("body" := _)
  , mode: ("mode" := _)
  , credentials: ("credentials" := _)
  , cache: ("cache" := _)
  , redirect: ("redirect" := _)
  , referrer: ("referrer" := _)
  , referrerPolicy: ("referrerPolicy" := _)
  , integrity: ("integrity" := _)
  , keepAlive: ("keepalive" := _)
  }


foreign import win_fetch_foreign
  :: String
  -> Obj
  -> Window
  -> Effect (Promise Response TypeError)

win_fetch :: String -> Window -> Effect (Promise Response TypeError)
win_fetch url w = win_fetch_foreign url empty w

win_fetch' :: String -> Options -> Window -> Effect (Promise Response TypeError)
win_fetch' url opts w = win_fetch_foreign url (options opts) w
