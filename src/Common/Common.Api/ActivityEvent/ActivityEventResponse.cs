/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Api.ActivityEvent
{
    public class ActivityEventResponse
    {
        public IEnumerable<object> ActivityEventEntities { get; set; }

        public string ContinuationToken { get; set; }

        public static implicit operator ActivityEventResponse(PowerBI.Api.V2.Models.ActivityEventResponse activityEventResponse)
        {
            if (activityEventResponse == null)
            {
                return null;
            }

            return new ActivityEventResponse
            {
                ActivityEventEntities = activityEventResponse.ActivityEventEntities,
                ContinuationToken = activityEventResponse.ContinuationToken
            };
        }
    }
}
