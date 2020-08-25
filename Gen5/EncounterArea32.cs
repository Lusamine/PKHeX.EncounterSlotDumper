﻿using System;

namespace PKHeX.EncounterSlotDumper
{
    public abstract class EncounterArea32 : EncounterArea
    {
        /// <summary>
        /// Gets an array of areas from an array of raw area data
        /// </summary>
        /// <param name="entries">Simplified raw format of an Area</param>
        /// <returns>Array of areas</returns>
        public static A[] GetArray<A, S>(byte[][] entries)
            where A : EncounterArea32, new()
            where S : EncounterSlot, new()
        {
            var data = new A[entries.Length];
            for (int i = 0; i < data.Length; i++)
            {
                var loc = data[i] = new A();
                loc.LoadSlots<S>(entries[i]);
            }
            return data;
        }

        private void LoadSlots<S>(byte[] areaData) where S : EncounterSlot, new()
        {
            var count = (areaData.Length - 2) / 4;
            Location = BitConverter.ToInt16(areaData, 0);
            Slots = new EncounterSlot[count];
            for (int i = 0; i < Slots.Length; i++)
            {
                int ofs = 2 + (i * 4);
                ushort SpecForm = BitConverter.ToUInt16(areaData, ofs);
                Slots[i] = new S
                {
                    Species = SpecForm & 0x7FF,
                    Form = SpecForm >> 11,
                    LevelMin = areaData[ofs + 2],
                    LevelMax = areaData[ofs + 3],
                };
            }
        }
    }
}