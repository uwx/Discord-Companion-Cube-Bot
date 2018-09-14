﻿// This file is a part of Companion Cube project.
// 
// Copyright 2018 Emzi0767
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using NpgsqlTypes;

namespace Emzi0767.CompanionCube.Data
{
    /// <summary>
    /// Represents kind of an entity with associated ID.
    /// </summary>
    public enum DatabaseEntityKind
    {
        /// <summary>
        /// Defines that the entity is a user.
        /// </summary>
        [PgName("user")]
        User,

        /// <summary>
        /// Defines that the entity is a channel.
        /// </summary>
        [PgName("channel")]
        Channel,

        /// <summary>
        /// Defines that the entity is a guild.
        /// </summary>
        [PgName("guild")]
        Guild
    }

    /// <summary>
    /// Represents kind of a tag in the database.
    /// </summary>
    public enum DatabaseTagKind
    {
        /// <summary>
        /// Defines that the tag is bound to a channel.
        /// </summary>
        [PgName("channel")]
        Channel,

        /// <summary>
        /// Defines that the tag is bound to a guild.
        /// </summary>
        [PgName("guild")]
        Guild,

        /// <summary>
        /// Defines that the tag is not bound, and it will appear and be usable everywhere.
        /// </summary>
        [PgName("global")]
        Global
    }
}
