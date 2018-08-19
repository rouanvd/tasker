module Webfu.DOM.Promise
( Promise
, mkPromise
, mkResolve
, mkReject
, thn
, thn'
, thn_
, catch_
, finally_
, race
, all
) where

import Prelude (Unit, unit, bind, (>>=), pure)
import Effect (Effect)
import Data.Function.Uncurried (Fn1, runFn1, Fn2, runFn2)


foreign import data Promise :: Type -> Type -> Type


--------------------------------------------------------------------------------
-- mkPromise
--------------------------------------------------------------------------------

foreign import mkPromiseImpl
  :: forall a b
   . Fn1 ((a -> Effect Unit) -> (b -> Effect Unit) -> Effect Unit)
         (Effect (Promise a b))

-- | Creates a new Promise, accepting an *executor* function.
-- |
-- | The *executor* function accepts 2 functions as arguments:
-- |  - resolve: a function that resolves the promise with a success value.
-- |  - reject: a function that rejects the promise with an error value.
-- |
-- | The executor function normally performs some asynchronouse work, and is immediately executed by the
-- | Promise implementation.  If the async operation was successful, we use the `resolve` function to
-- | signal this, supplying a success value.  If the async operation failed, we use the `reject` function
-- | to signal failure, supplying a reason of what went wrong.
-- |
-- | ```purescript
-- | mkPromise (\ resolveF rejectF -> do
-- |            jsonValue <- fetchJsonValue "http://example.com/dummyJsonValue"
-- |            if jsonValue == ""
-- |              then rejectF "no value found"
-- |              else resolveF jsonValue )
-- | ```
mkPromise
  :: forall a b
   . ((a -> Effect Unit) -> (b -> Effect Unit) -> Effect Unit)
  -> Effect (Promise a b)
mkPromise = runFn1 mkPromiseImpl


--------------------------------------------------------------------------------
-- then
--------------------------------------------------------------------------------

foreign import thnImpl
  :: forall a b
   . Fn2 (a -> Effect Unit)
         (Promise a b)
         (Effect (Promise a b))

thn :: forall a b
       . (a -> Effect Unit)
      -> Promise a b
      -> Effect (Promise a b)
thn = runFn2 thnImpl


--------------------------------------------------------------------------------
-- then'
--------------------------------------------------------------------------------

foreign import thnPrimeImpl
  :: forall a b c d
   . Fn2 (a -> Effect (Promise c d))
         (Promise a b)
         (Effect (Promise c d))

thn' :: forall a b c d
       . (a -> Effect (Promise c d))
      -> Promise a b
      -> Effect (Promise c d)
thn' = runFn2 thnPrimeImpl


--------------------------------------------------------------------------------
-- then_
--------------------------------------------------------------------------------

thn_ :: forall a b
       . (a -> Effect Unit)
      -> Promise a b
      -> Effect (Promise Unit b)
thn_ actionF p =
  pure p >>= (thn' (\v -> do _ <- actionF v
                             mkResolve unit))


--------------------------------------------------------------------------------
-- catch_
--------------------------------------------------------------------------------

foreign import catchImpl
  :: forall a b
   . Fn2 (b -> Effect Unit)
         (Promise a b)
         (Effect (Promise a b))

catch_ :: forall a b
        . (b -> Effect Unit)
       -> Promise a b
       -> Effect (Promise a b)
catch_ = runFn2 catchImpl



--------------------------------------------------------------------------------
-- finally_
--------------------------------------------------------------------------------

foreign import finallyImpl
  :: forall a b
   . Fn2 (Unit -> Effect Unit)
         (Promise a b)
         (Effect (Promise a b))

finally_ :: forall a b
        . (Unit -> Effect Unit)
       -> Promise a b
       -> Effect (Promise a b)
finally_ = runFn2 finallyImpl



--------------------------------------------------------------------------------
-- mkReject
--------------------------------------------------------------------------------

foreign import mkRejectImpl
  :: forall a b
   . Fn1 b
         (Effect (Promise a b))

mkReject :: forall a b
          . b
         -> Effect (Promise a b)
mkReject = runFn1 mkRejectImpl



--------------------------------------------------------------------------------
-- mkResolve
--------------------------------------------------------------------------------

foreign import mkResolveImpl
  :: forall a b
   . Fn1 a
         (Effect (Promise a b))

mkResolve :: forall a b
           . a
          -> Effect (Promise a b)
mkResolve = runFn1 mkResolveImpl



--------------------------------------------------------------------------------
-- race
--------------------------------------------------------------------------------

foreign import raceImpl
  :: forall a b
   . Fn1 (Array (Promise a b))
         (Effect (Promise a b))

race :: forall a b
      . Array (Promise a b)
     -> Effect (Promise a b)
race = runFn1 raceImpl



--------------------------------------------------------------------------------
-- all
--------------------------------------------------------------------------------

foreign import allImpl
  :: forall a b
   . Fn1 (Array (Promise a b))
         (Effect (Promise a b))

all :: forall a b
     . Array (Promise a b)
    -> Effect (Promise a b)
all = runFn1 allImpl
