module Webfu.Data.Err
  ( Err(..)
  ) where

import Prelude (class Show)

newtype Err = Err String

derive newtype instance showErr :: Show Err
